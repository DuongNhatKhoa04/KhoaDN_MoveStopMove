using System.Net.Mime;
using MoveStopMove.Core;
using MoveStopMove.Core.CoreComponents;
using MoveStopMove.DataPersistence;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Interfaces;
using UnityEngine;

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
        //[SerializeField] private Texture2D pantsAlbedoTexture;

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
            m_decoratorChain = new PantDecorator(
                                    new NullDecoratable())
            {
                PantsRenderer = pantsRenderer,
                PantTexture = GetPantTexture(DataPersistenceManager.Instance.PlayerGameData.equippedPant)
            };


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
            PantData pantSo = Resources.Load<PantData>($"SO/Pants/{pantName}");
            if (pantSo != null)
            {
                return pantSo.texture;
            }

            Debug.LogWarning($"PantData {pantName} not found!");
            return null;
        }

        private void GetHair()
        {

        }
    }
}