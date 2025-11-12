using System.Net.Mime;
using MoveStopMove.Core;
using MoveStopMove.Core.CoreComponents;
using MoveStopMove.DataPersistence;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.MainCharacter
{
    public class Player : Character, IDataPersistence
    {
        #region -- Fields --

        private Vector3 m_direction;
        private bool m_isMoving;
        private bool m_isGrounded;

        [Header("Pants")]
        [SerializeField] private SkinnedMeshRenderer pantsRenderer;
        [SerializeField] private SkinnedMeshRenderer skinRenderers;
        [SerializeField] private Material defaultSkinMaterial;

        private IDecoratable m_decoratorChain;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Initialize();
        }

        private void Start()
        {
            StateMachine.Initialize(CharacterIdleState);
            DecorateCharacter();
            m_decoratorChain.EquipPant();

            /*Debug.Log(DataPersistenceManager.Instance.PlayerGameData.equippedPant);
            Debug.Log(GetPantTexture(DataPersistenceManager.Instance.PlayerGameData.equippedPant));*/
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #endregion

        public void LoadData(GameData data)
        {
            Debug.Log("Loaded Pant: " + data.equippedPant);
        }

        public void SaveData(GameData data)
        {
            data.equippedPant = "chambi";
        }

        private Texture2D GetPantTexture(string pantName)
        {
            /*PantData pantSo = Resources.Load<PantData>($"SO/Pants/{pantName}");
            if (pantSo != null)
            {
                return pantSo.texture;
            }

            Debug.LogWarning($"PantData {pantName} not found!");
            return null;*/
            return GetDecoratorData<PantData, Texture2D>(pantName, "SO/Pants", data => data.texture);
        }


        private TResult GetDecoratorData<TData, TResult>(string itemName, string path,
                                                            System.Func<TData, TResult> selector)
            where TData : ScriptableObject
        {
            TData dataSo = Resources.Load<TData>($"{path}/{itemName}");

            if (dataSo != null)
            {
                return selector(dataSo);
            }

            Debug.LogWarning($"[GetDecoratorData] Không tìm thấy {typeof(TData).Name} tại {path}/{itemName}");
            return default;
        }

        private void DecorateCharacter()
        {
            /*m_decoratorChain = new WeaponDecorator(new HairDecorator(new WingDecorator(new TailDecorator(new PantDecorator(new SkinDecorator(new NullDecoratable()))))))
            {
                DefaultSkinMaterial = defaultSkinMaterial,
                PantsRenderer = pantsRenderer,
                PantTexture = GetPantTexture(DataPersistenceManager.Instance.PlayerGameData.equippedPant)
            };*/

            var nullDecor = new NullDecoratable();
            var skinDecorator = new SkinDecorator(nullDecor)
            {
                SkinSetRenderer = skinRenderers,
                DefaultSkinMaterial = defaultSkinMaterial
            };

            var pantDecorator = new PantDecorator(skinDecorator)
            {
                PantsRenderer = pantsRenderer,
                PantTexture = GetPantTexture(DataPersistenceManager.Instance.PlayerGameData.equippedPant)
            };

            var tailDecorator = new TailDecorator(pantDecorator);
            var wingDecorator = new WingDecorator(tailDecorator);
            var hairDecorator = new HairDecorator(wingDecorator);
            var weaponDecorator = new WeaponDecorator(hairDecorator);

            m_decoratorChain = weaponDecorator;
        }
    }
}