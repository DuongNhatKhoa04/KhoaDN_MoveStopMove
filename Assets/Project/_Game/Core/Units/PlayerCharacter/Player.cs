using System.Collections;
using MoveStopMove.Core.Appearance;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Core.StateMachine.PlayerState;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility;
using MoveStopMove.Utility.Audio;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.Units.PlayerCharacter
{
    public class Player : Character, IDataPersistence, IDamageable,
        IMyObserver<ItemEquippedEvent>, IMyObserver<HitTarget>, IMyObserver<ItemTryEvent>, IMyObserver<ItemCancelTryEvent>
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

        private bool m_isPreviewing;
        private string m_previewOriginWeapon;
        private string m_previewOriginHair;
        private string m_previewOriginPant;
        private string m_previewOriginTail;
        private string m_previewOriginWing;
        private string m_previewOriginCustom;

        private float m_rangeUp;
        private int m_currentCoin;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            StartCoroutine(InitializeRoutine());
            base.Initialize();
        }

        private IEnumerator Start()
        {
            yield return this.WaitForGameDataLoaded();

            m_currentCoin = DataPersistenceManager.Instance.GameData.coins;
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

        public override void InitStateMachine()
        {
            base.InitStateMachine();
            PlayerIdleState = new PlayerIdleState(this, StateMachine, characterData, EAnim.Idle);
            PlayerMoveState = new PlayerMoveState(this, StateMachine, characterData, EAnim.Run);
            PlayerAttackState = new PlayerAttackState(this, StateMachine, characterData, EAnim.Attack);
            PlayerDeadState = new PlayerDeadState(this, StateMachine, characterData, EAnim.Dead);
            PlayerDanceState = new PlayerDanceState(this, StateMachine, characterData, EAnim.Dance);
        }

        #region - Player Data -

        public void LoadData(GameData data)
        {
            m_gameData = data;
        }

        public void SaveData(GameData data)
        {
            m_gameData = data;
        }

        #endregion

        #region - Decoration -

        private CustomVisualContext BuildCustomContext(string customName)
        {
            var context = new CustomVisualContext();

            if (string.IsNullOrEmpty(customName))
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
                m_pantDecorator.PantTexture = pantData.texture;
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

            if (!string.IsNullOrEmpty(customName))
            {
                if (customData.hasWeapon)
                {
                    m_weaponDecorator.WeaponPrefab = customData.weaponPrefab;
                    m_weaponDecorator.ProjectilePrefab = customData.projectile;
                    m_weaponDecorator.EquipWeapon();
                }

                if (customData.hasHair)
                {
                    m_hairDecorator.HairPrefab = customData.hairPrefab;
                    m_hairDecorator.EquipHair();
                }

                if (customData.hasPant)
                {
                    m_pantDecorator.PantTexture = customData.pant;
                    m_pantDecorator.EquipPant();
                }

                if (customData.hasTail)
                {
                    m_tailDecorator.TailPrefab = customData.tailPrefab;
                    m_tailDecorator.EquipTail();
                }

                if (customData.hasWing)
                {
                    m_wingDecorator.WingPrefab = customData.wingPrefab;
                    m_wingDecorator.EquipWing();
                }

                if (customData.hasSkinTexture)
                {
                    m_skinDecorator.HasTexture = customData.hasSkinTexture;
                    m_skinDecorator.SkinTexture = customData.skinTexture;
                    m_skinDecorator.EquipSkin();
                }
                else
                {
                    m_skinDecorator.HasTexture = customData.hasSkinTexture;
                    m_skinDecorator.SkinMaterial = customData.skinMaterial;
                    m_skinDecorator.EquipSkin();
                }
            }
            else
            {
                EquipWeaponRuntime(m_gameData.equippedWeapon);
                EquipHairRuntime(m_gameData.equippedHair);
                EquipPantRuntime(m_gameData.equippedPant);

                if (m_tailDecorator != null)
                {
                    m_tailDecorator.TailPrefab = null;
                    m_tailDecorator.EquipTail();
                }

                if (m_wingDecorator != null)
                {
                    m_wingDecorator.WingPrefab = null;
                    m_wingDecorator.EquipWing();
                }

                if (m_skinDecorator != null)
                {
                    m_skinDecorator.HasTexture   = false;
                    m_skinDecorator.SkinTexture  = null;
                    m_skinDecorator.SkinMaterial = defaultSkinMaterial;
                    m_skinDecorator.EquipSkin();
                }
            }
        }

        #region -- Try Decorator --

        /// <summary>
        /// Preview item in shop
        /// </summary>
        /// <param name="itemType">Custom/Hair/Pant/Weapon</param>
        /// <param name="itemName">Name of item</param>
        private void PreviewItem(EItem itemType, string itemName)
        {
            if (!m_isInitialized)
                return;

            if (!m_isPreviewing)
            {
                m_previewOriginWeapon = m_gameData.equippedWeapon;
                m_previewOriginHair = m_gameData.equippedHair;
                m_previewOriginPant = m_gameData.equippedPant;
                m_previewOriginWing = null;
                m_previewOriginTail = null;
                m_previewOriginCustom = m_gameData.equippedCustom;
                m_isPreviewing = true;
            }

            switch (itemType)
            {
                case EItem.Weapon:
                    EquipWeaponRuntime(itemName);
                    break;

                case EItem.Hair:
                    EquipHairRuntime(itemName);
                    break;

                case EItem.Pant:
                    EquipPantRuntime(itemName);
                    break;

                case EItem.Custom:
                    EquipCustomRuntime(itemName);
                    break;
            }
        }

        /// <summary>
        /// Cancel previewing mode in shop
        /// </summary>
        private void CancelPreview()
        {
            if (!m_isPreviewing)
                return;

            if (!string.IsNullOrEmpty(m_previewOriginCustom))
            {
                EquipCustomRuntime(m_previewOriginCustom);
            }
            else
            {
                if (!string.IsNullOrEmpty(m_previewOriginWeapon))
                    EquipWeaponRuntime(m_previewOriginWeapon);

                if (!string.IsNullOrEmpty(m_previewOriginHair))
                    EquipHairRuntime(m_previewOriginHair);

                if (!string.IsNullOrEmpty(m_previewOriginPant))
                    EquipPantRuntime(m_previewOriginPant);

                if (string.IsNullOrEmpty(m_previewOriginWing))
                {
                    m_wingDecorator.WingPrefab = null;
                    m_wingDecorator.EquipWing();
                }

                if (string.IsNullOrEmpty(m_previewOriginTail))
                {
                    m_tailDecorator.TailPrefab = null;
                    m_tailDecorator.EquipTail();
                }

                if (m_skinDecorator != null)
                {
                    m_skinDecorator.HasTexture   = false;
                    m_skinDecorator.SkinTexture  = null;
                    m_skinDecorator.SkinMaterial = defaultSkinMaterial;
                    m_skinDecorator.EquipSkin();
                }
            }

            m_isPreviewing = false;
        }

        #endregion


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
            EventManager.Instance.Subscribe<ItemTryEvent>(this);
            EventManager.Instance.Subscribe<ItemCancelTryEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.Instance.Unsubscribe<HitTarget>(this);
            EventManager.Instance.Unsubscribe<ItemEquippedEvent>(this);
            EventManager.Instance.Unsubscribe<ItemTryEvent>(this);
            EventManager.Instance.Unsubscribe<ItemCancelTryEvent>(this);
        }

        public void OnNotify(HitTarget data)
        {
            if (data.Victim.name == "Character")
            {
                m_rangeUp = DataPersistenceManager.Instance.MaxRangeIncrease;
                base.UpdateRange(m_rangeUp);

                DataPersistenceManager.Instance.GameData.coins = m_currentCoin++;

                SoundManager.Instance.PlaySFX(ESfxType.ProjectileHit);

                var enemy = data.Target.GetComponent<Character>();
                ObjectPoolingManager.Instance.ReleaseObjectToPool(enemy, "EnemyPool");
            }
            else if (data.Victim.name == "Enemy")
            {
                TakeHit();
            }
        }

        /// <summary>
        /// Equip item in runtime
        /// </summary>
        /// <param name="data">Data of event</param>
        public void OnNotify(ItemEquippedEvent data)
        {
            UpdateEquippedNameInGameData(data);
            ApplyEquippedVisualRuntime(data);
        }

        /// <summary>
        /// Try item in runtime
        /// </summary>
        /// <param name="data">Data of event</param>
        public void OnNotify(ItemTryEvent data)
        {
            CancelPreview();
            PreviewItem(data.ItemType, data.ItemName);
            StateMachine.ChangeState(PlayerDanceState);
        }

        /// <summary>
        /// Cancel try item in runtime
        /// </summary>
        /// <param name="data">Data of even</param>
        public void OnNotify(ItemCancelTryEvent data)
        {
            CancelPreview();
        }

        #endregion
    }
}