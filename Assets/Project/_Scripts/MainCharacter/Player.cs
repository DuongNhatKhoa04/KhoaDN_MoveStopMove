using MoveStopMove.Core;
using MoveStopMove.DataPersistence;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Extensions.Decorator;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public class Player : Character, IDataPersistence
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

        private WeaponDecorator m_weaponDecorator;
        private HairDecorator m_hairDecorator;
        private WingDecorator m_wingDecorator;
        private TailDecorator m_tailDecorator;
        private PantDecorator m_pantDecorator;
        private SkinDecorator m_skinDecorator;

        private CustomVisualContext m_customContext;
        private GameData m_gameData;

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

            StateMachine.Initialize(CharacterIdleState);

            InitializationDecorator();
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        private void InitializationDecorator()
        {
            var nullDecorator = new NullDecoratable();
            var data = m_gameData;

            m_skinDecorator = new SkinDecorator(nullDecorator)
            {
                SkinSetRenderer     = skinRenderers,
                DefaultSkinMaterial = defaultSkinMaterial,
                SkinMaterial        = m_customContext.skinMaterial,
                SkinTexture         = m_customContext.skinTexture,
                HasTexture          = m_customContext.hasTextureInSkin,
            };

            var pantTexture = m_customContext.pantTexture;
            if (pantTexture == null && !string.IsNullOrEmpty(m_gameData.equippedPant) && data.equippedPant != "none")
            {
                pantTexture = GetPantTexture(data.equippedPant);
            }

            m_pantDecorator = new PantDecorator(m_skinDecorator)
            {
                PantsRenderer = pantsRenderer,
                PantTexture   = pantTexture
            };

            m_tailDecorator = new TailDecorator(m_pantDecorator)
            {
                TailAttachment = tailAttachment,
                TailPrefab     = GetTailPrefab()
            };

            m_wingDecorator = new WingDecorator(m_tailDecorator)
            {
                WingAttachment = wingAttachment,
                WingPrefab     = GetWingPrefab()
            };

            m_hairDecorator = new HairDecorator(m_wingDecorator)
            {
                HairAttachment = hairAttachment,
                HairPrefab     = GetHairPrefab()
            };

            m_weaponDecorator = new WeaponDecorator(m_hairDecorator)
            {
                WeaponAttachment = weaponAttachment,
                WeaponPrefab     = GetWeaponPrefab()
            };

            m_decoratorChain = m_weaponDecorator;

            m_decoratorChain.EquipSkin();
            m_decoratorChain.EquipPant();
            m_decoratorChain.EquipHair();
            m_decoratorChain.EquipWing();
            m_decoratorChain.EquipTail();
            m_decoratorChain.EquipWeapon();
        }

        private void ApplyVisual()
        {

        }

        #region - Player Data -

        public void LoadData(GameData data)
        {
            Debug.Log("Loaded Pant: " + data);
        }

        public void SaveData(GameData data)
        {
            data.equippedPant = "chambi";
        }

        #endregion

        #region - Get data for decoration -

        #region - Weapon -

        private GameObject GetWeaponPrefab()
        {
            GameObject prefab = null;

            if (m_customContext.weaponPrefab != null)
            {
                prefab = m_customContext.weaponPrefab;
                return prefab;
            }

            prefab = GetWeaponObject(m_gameData.equippedWeapon);
            return prefab;
        }

        private GameObject GetWeaponObject(string weaponName)
        {
            return PlayerSaveLoader.GetDecoratorData<WeaponData, GameObject>(
                weaponName,
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data.prefab);
        }

        #endregion

        #region - Hair -

        private GameObject GetHairPrefab()
        {
            GameObject prefab = null;

            if (m_customContext.hairPrefab != null)
            {
                prefab = m_customContext.hairPrefab;
                return prefab;
            }

            prefab = GetHairObject(m_gameData.equippedHair);
            return prefab;
        }

        private GameObject GetHairObject(string hairName)
        {
            if (hairName == "none") return null;

            return PlayerSaveLoader.GetDecoratorData<HairData, GameObject>(
                hairName,
                PlayerSaveLoader.SO_HAIRS_PATH,
                data => data.prefab);
        }

        #endregion

        #region - Wing -

        private GameObject GetWingPrefab()
        {
            GameObject prefab = null;

            if (m_customContext.wingPrefab == null) return null;

            prefab = m_customContext.wingPrefab;
            return prefab;
        }

        #endregion

        #region - Tail -

        private GameObject GetTailPrefab()
        {
            GameObject prefab = null;

            if (m_customContext.tailPrefab == null) return null;

            prefab = m_customContext.tailPrefab;
            return prefab;
        }

        #endregion

        #region - Pant -

        private Texture2D GetPantTexture(string pantName)
        {
            if (pantName == "none") return null;

            return PlayerSaveLoader.GetDecoratorData<PantData, Texture2D>(
                pantName,
                PlayerSaveLoader.SO_PANTS_PATH,
                data => data.texture);
        }

        #endregion

        #region - Skin -

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
            context.hairPrefab = customData.hasHair ? customData.hairPrefab : null;
            context.wingPrefab = customData.hasWing ? customData.wingPrefab : null;
            context.tailPrefab = customData.hasTail ? customData.tailPrefab : null;

            return context;
        }

        #endregion

        #endregion

        #region - Change custom in runtime -

        public void ChangePant(string pantName, bool save = true)
        {
            var newPantTexture = GetPantTexture(pantName);
            if (newPantTexture == null)
            {
                Debug.LogWarning($"[Player] Pant '{pantName}' not found.");
                return;
            }

            m_pantDecorator.PantTexture = newPantTexture;

            m_decoratorChain.EquipPant();

            if (!save) return;

            DataPersistenceManager.Instance.PlayerGameData.equippedPant = pantName;
            DataPersistenceManager.Instance.SaveGame();
        }

        #endregion

        #endregion
    }
}