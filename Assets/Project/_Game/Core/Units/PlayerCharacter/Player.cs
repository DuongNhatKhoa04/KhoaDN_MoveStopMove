using System.Collections;
using MoveStopMove.Core.Appearance;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.Units.PlayerCharacter
{
    public class Player : Character, IDataPersistence, IMyObserver<HitTarget>, IDamageable, IMyObserver<ItemEquippedEvent>
    {
        #region -- Fields --

        [Header("Skinned Mesh Renderer")]
        [SerializeField] private SkinnedMeshRenderer pantsRenderer;
        [SerializeField] private SkinnedMeshRenderer skinRenderers;

        [SerializeField] private Material defaultSkinMaterial;

        [Header("Attachment Decorator")]
        [SerializeField] private GameObject weaponAttachment;
        [SerializeField] private GameObject hairAttachment;
        [SerializeField] private GameObject wingAttachment;
        [SerializeField] private GameObject tailAttachment;

        private Vector3 m_direction;
        private bool m_isMoving;
        private bool m_isGrounded;
        private bool m_isInitialized;

        private IDecoratable m_decoratorChain;
        private CustomVisualContext m_customContext;
        private GameData m_gameData;
        private PlayerVisualProvider m_playerVisualProvider;
        private RendererReferences m_renderRefs;
        private AttachmentReferences m_attachRefs;

        private WeaponDecorator m_weaponDecorator;
        private HairDecorator m_hairDecorator;
        private WingDecorator m_wingDecorator;
        private TailDecorator m_tailDecorator;
        private PantDecorator m_pantDecorator;
        private SkinDecorator m_skinDecorator;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Initialize();
        }

        private IEnumerator Start()
        {
            yield return this.WaitForGameDataLoaded();

            m_gameData = DataPersistenceManager.Instance.GameData;
            m_customContext = BuildCustomContext(m_gameData.equippedCustom);

            m_playerVisualProvider = new PlayerVisualProvider(m_customContext, defaultSkinMaterial);

            m_renderRefs = new RendererReferences
            {
                PantsRenderer = pantsRenderer,
                SkinRenderer = skinRenderers
            };

            m_attachRefs = new AttachmentReferences
            {
                WeaponAttachment = weaponAttachment,
                HairAttachment = hairAttachment,
                WingAttachment = wingAttachment,
                TailAttachment = tailAttachment
            };

            m_decoratorChain = CharacterDecoratorBuilder.Build(
                Core,
                m_gameData,
                m_customContext,
                m_renderRefs,
                m_attachRefs,
                m_playerVisualProvider
            );

            m_weaponDecorator = FindDecoratorInChain<WeaponDecorator>(m_decoratorChain);
            m_hairDecorator = FindDecoratorInChain<HairDecorator>(m_decoratorChain);
            m_wingDecorator = FindDecoratorInChain<WingDecorator>(m_decoratorChain);
            m_tailDecorator = FindDecoratorInChain<TailDecorator>(m_decoratorChain);
            m_pantDecorator = FindDecoratorInChain<PantDecorator>(m_decoratorChain);
            m_skinDecorator = FindDecoratorInChain<SkinDecorator>(m_decoratorChain);

            m_decoratorChain.EquipSkin();
            m_decoratorChain.EquipPant();
            m_decoratorChain.EquipHair();
            m_decoratorChain.EquipWing();
            m_decoratorChain.EquipTail();
            m_decoratorChain.EquipWeapon();

            StateMachine.Initialize(PlayerIdleState);
            m_isInitialized = true;
        }

        private void Update()
        {
            if (!m_isInitialized || StateMachine.CurrentState == null)
                return;

            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            if (!m_isInitialized || StateMachine.CurrentState == null)
                return;

            StateMachine.CurrentState.PhysicsUpdate();
        }

        #region - Player Data -

        public void LoadData(GameData data)
        {
            m_gameData = data;
        }

        public void SaveData(GameData data)
        {
            data.equippedPant = "chambi";
        }

        #endregion

        #region - Decoration -

        private CustomVisualContext BuildCustomContext(string customName)
        {
            var context = new CustomVisualContext();

            if (customName == "none")
            {
                context.customData = null;

                context.hasTextureInSkin = false;
                context.skinTexture = null;
                context.skinMaterial = defaultSkinMaterial;

                context.pantTexture = null;

                context.weaponPrefab = null;
                context.projectilePrefab = null;
                context.hairPrefab = null;
                context.wingPrefab = null;
                context.tailPrefab = null;

                return context;
            }

            var customData = PlayerSaveLoader.GetDecoratorData<CustomData, CustomData>(
                customName,
                PlayerSaveLoader.SO_CUSTOMS_PATH,
                data => data);

            context.customData = customData;

            if (customData == null)
            {
                context.skinMaterial = defaultSkinMaterial;
                context.hasTextureInSkin = false;
                return context;
            }

            context.hasTextureInSkin = customData.hasSkinTexture && customData.skinTexture != null;

            if (context.hasTextureInSkin)
            {
                context.skinTexture = customData.skinTexture;
                context.skinMaterial = defaultSkinMaterial;
            }
            else
            {
                context.skinTexture = null;
                context.skinMaterial = customData.skinMaterial;
            }

            context.pantTexture = customData.hasPant ? customData.pant : null;
            context.weaponPrefab = customData.hasWeapon ? customData.weaponPrefab : null;
            context.projectilePrefab = customData.projectile ? customData.projectile : null;
            context.hairPrefab = customData.hasHair ? customData.hairPrefab : null;
            context.wingPrefab = customData.hasWing ? customData.wingPrefab : null;
            context.tailPrefab = customData.hasTail ? customData.tailPrefab : null;

            return context;
        }

        private T FindDecoratorInChain<T>(IDecoratable root) where T : class, IDecoratable
        {
            var current = root;
            while (current is CharacterDecorator decorator)
            {
                if (current is T match)
                    return match;

                current = decorator.Inner;
            }
            return null;
        }

        private void UpdateEquippedNameInGameData(ItemEquippedEvent data)
        {
            switch (data.ItemType)
            {
                case EItem.Weapon:
                    m_gameData.equippedWeapon = data.ItemName;
                    break;

                case EItem.Hair:
                    m_gameData.equippedHair = data.ItemName;
                    break;

                case EItem.Pant:
                    m_gameData.equippedPant = data.ItemName;
                    break;

                case EItem.Custom:
                    m_gameData.equippedCustom = data.ItemName;
                    break;
            }
        }

        private void ApplyEquippedVisualRuntime(ItemEquippedEvent data)
        {
            switch (data.ItemType)
            {
                case EItem.Weapon:
                    EquipWeaponRuntime(data.ItemName);
                    break;

                case EItem.Hair:
                    EquipHairRuntime(data.ItemName);
                    break;

                case EItem.Pant:
                    EquipPantRuntime(data.ItemName);
                    break;

                case EItem.Custom:
                    EquipCustomRuntime(data.ItemName);
                    break;
            }
        }

        private void EquipWeaponRuntime(string weaponName)
        {
            var weaponData = PlayerSaveLoader.GetDecoratorData<WeaponData, WeaponData>(
                weaponName,
                PlayerSaveLoader.SO_WEAPON_PATH,
                w => w
            );

            if (weaponData != null && m_weaponDecorator != null)
            {
                m_weaponDecorator.WeaponAttachment = weaponAttachment;

                m_weaponDecorator.WeaponPrefab     = weaponData.prefab;
                m_weaponDecorator.ProjectilePrefab = weaponData.projectilePrefab;

                m_weaponDecorator.EquipWeapon();
            }
        }

        private void EquipHairRuntime(string hairName)
        {
            var hairData = PlayerSaveLoader.GetDecoratorData<HairData, HairData>(
                hairName,
                PlayerSaveLoader.SO_HAIRS_PATH,
                hair => hair
            );

            if (hairData != null && m_hairDecorator != null)
            {
                m_hairDecorator.HairPrefab = hairData.prefab;
                m_hairDecorator.EquipHair();
            }
        }

        private void EquipPantRuntime(string pantName)
        {
            var pantData = PlayerSaveLoader.GetDecoratorData<PantData, PantData>(
                pantName,
                PlayerSaveLoader.SO_PANTS_PATH,
                pant => pant
            );

            if (pantData != null && m_pantDecorator != null)
            {
                m_pantDecorator.PantsRenderer = pantsRenderer;
                m_pantDecorator.PantTexture   = pantData.texture;
                m_pantDecorator.EquipPant();
            }
        }

        private void EquipCustomRuntime(string customName)
        {
            var customData = PlayerSaveLoader.GetDecoratorData<CustomData, CustomData>(
                customName,
                PlayerSaveLoader.SO_CUSTOMS_PATH,
                custom => custom
            );

            if (customData != null)
            {
                if (customData.hasWeapon)
                {
                    m_weaponDecorator.WeaponPrefab = customData.weaponPrefab;
                    m_weaponDecorator.ProjectilePrefab = customData.projectile;
                }

                if (customData.hasHair)
                {
                    m_hairDecorator.HairPrefab = customData.hairPrefab;
                }

                if (customData.hasPant)
                {
                    m_pantDecorator.PantTexture = customData.pant;
                }

                if (customData.hasTail)
                {
                    m_tailDecorator.TailPrefab = customData.tailPrefab;
                }

                if (customData.hasWing)
                {
                    m_wingDecorator.WingPrefab = customData.wingPrefab;
                }

                if (customData.hasSkinTexture)
                {
                    m_skinDecorator.HasTexture = customData.hasSkinTexture;
                    m_skinDecorator.SkinTexture = customData.skinTexture;
                }
                else
                {
                    m_skinDecorator.HasTexture = customData.hasSkinTexture;
                    m_skinDecorator.SkinMaterial = customData.skinMaterial;
                }
            }
        }

        #endregion

        public void TakeHit()
        {
            StateMachine.ChangeState(PlayerDeadState);
        }

        #endregion

        #region -- Observer --

        private void OnEnable()
        {
            EventManager.Instance.Subscribe<HitTarget>(this);
            EventManager.Instance.Subscribe<ItemEquippedEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.Instance.Unsubscribe<HitTarget>(this);
            EventManager.Instance.Unsubscribe<ItemEquippedEvent>(this);
        }

        public void OnNotify(HitTarget data)
        {
            Debug.Log("Defeated " + data.Target + ", increase attack range by " + data.RangeUpdate);
            core.Combat.GetAttackRange.IncreaseRange(data.RangeUpdate);
        }

        // Equip item in runtime
        public void OnNotify(ItemEquippedEvent data)
        {
            UpdateEquippedNameInGameData(data);
            ApplyEquippedVisualRuntime(data);
        }

        #endregion
    }
}