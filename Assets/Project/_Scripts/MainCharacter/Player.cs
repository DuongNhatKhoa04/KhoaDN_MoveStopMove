using System.IO;
using MoveStopMove.Core;
using MoveStopMove.Core.CoreComponents;
using MoveStopMove.DataPersistence;
using MoveStopMove.DataPersistence.Data;
using MoveStopMove.Extensions.FSM;
using MoveStopMove.Extensions.FSM.States;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using MoveStopMove.MainCharacter.Data;
using MoveStopMove.Manager;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public class Player : Character, IDataPersistence
    {
        #region -- Fields --

        private Vector3 m_direction;
        private bool m_isMoving;
        private bool m_isGrounded;

        [Header("Pants Skin Test")]
        [SerializeField] private SkinnedMeshRenderer pantsRenderer;
        [SerializeField] private Texture2D pantsAlbedoTexture;

        private IDecoratable m_decoratorChain;

        private GameData m_playerData;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Initialize();
            m_decoratorChain = new WeaponDecorator(new FullSetSkinDecorator(new PantDecorator(new NullDecoratable())))
            {
                PantsRenderer = pantsRenderer,
                PantTexture = pantsAlbedoTexture
            };
            m_playerData = GameData.CreateDefault();
        }

        private void Start()
        {
            StateMachine.Initialize(CharacterIdleState);
            m_decoratorChain.EquipPant();
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
            m_playerData = data;
        }

        public void SaveData(GameData data)
        {
            data = m_playerData;
        }
    }
}