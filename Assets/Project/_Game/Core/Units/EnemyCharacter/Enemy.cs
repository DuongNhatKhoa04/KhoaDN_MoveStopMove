using MoveStopMove.Core.Appearance;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.StateMachine.EnemyState;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace MoveStopMove.Core.Units.EnemyCharacter
{
    public class Enemy : Character
    {
        #region -- Fields --

        [Header("Skinned Mesh Renderer")]
        [SerializeField] private SkinnedMeshRenderer pantsRenderer;

        [Header("Attachment Decorator")]
        [SerializeField] private GameObject weaponAttachment;

        [SerializeField] private Texture2D pantTexture;

        private IDecoratable m_decoratorChain;

        #endregion

        #region -- Properties --

        public NavMeshAgent Agent { get; private set; }

        #endregion

        #region -- Methods --

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            base.Initialize();
        }

        public override void InitStateMachine()
        {
            base.InitStateMachine();

            EnemyIdleState = new EnemyIdleState(this, StateMachine, characterData, EAnim.Idle);
            EnemyMoveState = new EnemyMoveState(this, StateMachine, characterData, EAnim.Run);
            EnemyAttackState = new EnemyAttackState(this, StateMachine, characterData, EAnim.Attack);
            EnemyDeadState = new EnemyDeadState(this, StateMachine, characterData, EAnim.Dead);
        }

        private void Start()
        {
            StateMachine.Initialize(EnemyIdleState);

            var weaponData = PlayerSaveLoader.GetDecoratorData<WeaponData, WeaponData>(
                "z",
                PlayerSaveLoader.SO_WEAPON_PATH,
                data => data);

            var nullDeco = new NullDecoratable();

            var pant = new PantDecorator(nullDeco)
            {
                PantsRenderer = pantsRenderer,
                PantTexture = pantTexture
            };

            var weapon = new WeaponDecorator(pant)
            {
                WeaponAttachment = weaponAttachment,
                WeaponPrefab = weaponData.prefab,
                ProjectilePrefab = weaponData.projectilePrefab,
                Core = core
            };

            m_decoratorChain = weapon;

            m_decoratorChain.EquipPant();
            m_decoratorChain.EquipWeapon();
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }

        #endregion
    }
}