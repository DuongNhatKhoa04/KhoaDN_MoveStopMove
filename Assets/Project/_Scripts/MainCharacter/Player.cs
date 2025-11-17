using MoveStopMove.Core;
using MoveStopMove.DataPersistence;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Extensions.Observer;
using MoveStopMove.Interfaces;
using MoveStopMove.Managers;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public class Player : Character, IDataPersistence, IMyObserver<HitTarget>
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

        private IDecoratable m_decoratorChain;

        private CustomVisualContext m_customContext;
        private GameData m_gameData;

        private PlayerVisualProvider m_playerVisualProvider;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Initialize();
        }

        private void Start()
        {
            m_gameData = DataPersistenceManager.Instance.PlayerGameData;
            m_customContext = BuildCustomContext(m_gameData.equippedCustom);

            m_playerVisualProvider = new PlayerVisualProvider(m_customContext, defaultSkinMaterial);

            var renderRefs = new RendererReferences
            {
                PantsRenderer = pantsRenderer,
                SkinRenderer = skinRenderers
            };

            var attachRefs = new AttachmentReferences
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
                renderRefs,
                attachRefs,
                m_playerVisualProvider
            );

            m_decoratorChain.EquipSkin();
            m_decoratorChain.EquipPant();
            m_decoratorChain.EquipHair();
            m_decoratorChain.EquipWing();
            m_decoratorChain.EquipTail();
            m_decoratorChain.EquipWeapon();

            StateMachine.Initialize(CharacterIdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #region - Player Data -

        public void LoadData(GameData data)
        {
            Debug.Log("Loaded: " + data.equippedWeapon);
        }

        public void SaveData(GameData data)
        {
            data.equippedPant = "chambi";
        }

        #endregion

        #region - Get data for decoration -

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

        #endregion

        #endregion

        #region -- Observer --

        private void OnEnable()
        {
            EventManager.Instance?.Subscribe<HitTarget>(this);
        }

        private void OnDisable()
        {
            EventManager.Instance?.Unsubscribe<HitTarget>(this);
        }

        public void OnNotify(HitTarget data)
        {
            Debug.Log("Defeated " + data.Target + ", increase attack range by " + data.RangeUpdate);
            core.Combat.GetAttackRange.IncreaseRange(data.RangeUpdate);
        }

        #endregion
    }
}