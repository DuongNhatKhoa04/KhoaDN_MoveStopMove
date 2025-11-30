using System;
using System.Collections;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.StateMachine;
using MoveStopMove.Core.StateMachine.EnemyState;
using MoveStopMove.Core.StateMachine.PlayerState;
using MoveStopMove.Core.Stats;
using MoveStopMove.Gameplay.Projectiles;
using MoveStopMove.Utility.Extension;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Core.Units
{
    public abstract class Character : MonoBehaviour, IInitializable
    {
        #region -- Fields --

        [Header("Character Animation")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected EAnim currentAnimation;
        [SerializeField] protected MainCore core;
        [SerializeField] protected CharacterData characterData;

        protected IObjectPool<Character> CharacterPool;

        private int m_lastAttackLoop = -1;

        #endregion

        #region -- Properties --

        public MainCore Core => core;
        protected FiniteStateMachine StateMachine { get; set; }
        public PlayerIdleState PlayerIdleState { get; set; }
        public PlayerMoveState PlayerMoveState { get; set; }
        public PlayerAttackState PlayerAttackState {  get; set; }
        public PlayerDeadState PlayerDeadState { get; set; }
        public PlayerDanceState PlayerDanceState { get; set; }

        public EnemyIdleState EnemyIdleState { get; set; }
        public EnemyMoveState EnemyMoveState { get; set; }
        public EnemyAttackState EnemyAttackState {  get; set; }
        public EnemyDeadState EnemyDeadState { get; set; }

        public IObjectPool<Character> ObjectPool
        {
            set => CharacterPool = value;
        }

        #endregion

        #region -- Methods --

        public virtual void Initialize()
        {
            //StartCoroutine(InitializeRoutine());
            InitAttackRange(characterData.attackRangeRadius);
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
            InitStateMachine();
        }

        protected IEnumerator InitializeRoutine()
        {
            yield return this.WaitForGameDataLoaded();
            DataPersistenceManager.Instance.UpdateMaxRange();
            DataPersistenceManager.Instance.UpdateRangeIncreasement();
            DataPersistenceManager.Instance.UpdateMaxMovement();
            /*InitAttackRange(characterData.attackRangeRadius);
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
            InitStateMachine();*/
        }

        private void InitAttackRange(float initRange)
        {
            core.Battle.GetAttackRange.InitRange(initRange);
        }

        protected void UpdateRange(float rangeIncrease)
        {
            core.Battle.GetAttackRange.IncreaseRange(rangeIncrease);
        }

        public virtual void InitStateMachine()
        {
            StateMachine = new FiniteStateMachine();

            /*PlayerIdleState = new PlayerIdleState(this, StateMachine, characterData, EAnim.Idle);
            PlayerMoveState = new PlayerMoveState(this, StateMachine, characterData, EAnim.Run);
            PlayerAttackState = new PlayerAttackState(this, StateMachine, characterData, EAnim.Attack);
            PlayerDeadState = new PlayerDeadState(this, StateMachine, characterData, EAnim.Dead);*/
        }

        public void ChangeAnimation(EAnim animationName, float speed = 1)
        {
            animator.speed = Mathf.Max(0f, speed);

            if (currentAnimation == animationName) return;

            animator.ResetTrigger(AnimHashes.Map[currentAnimation]);
            currentAnimation = animationName;
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
        }

        public void SetAnimationTrigger(EAnim animationName)
        {
            animator.SetTrigger(AnimHashes.Map[animationName]);
        }

        public void ResetAnimationTrigger(EAnim animationName)
        {
            animator.ResetTrigger(AnimHashes.Map[animationName]);
        }

        public bool HasAnimationLooped(EAnim anim, out int loopIndex)
        {
            loopIndex = 0;

            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName(anim.ToString()))
            {
                m_lastAttackLoop = -1;
                return false;
            }

            int currentLoop = Mathf.FloorToInt(stateInfo.normalizedTime);

            if (currentLoop > m_lastAttackLoop)
            {
                loopIndex = currentLoop;
                m_lastAttackLoop = currentLoop;
                return true;
            }

            return false;
        }

        #endregion
    }

    public abstract class CharacterDecorator : IDecoratable
    {
        #region -- Fields --

        public MainCore Core;

        private readonly IDecoratable m_inner;
        private static readonly int s_mainTex = Shader.PropertyToID("_MainTex");

        #endregion

        #region -- Properties --

        public IDecoratable Inner => m_inner;
        public SkinnedMeshRenderer PantsRenderer { get; set; }
        public Texture2D PantTexture { get; set; }

        #endregion

        #region -- Methods --

        protected CharacterDecorator(IDecoratable inner)
        {
            m_inner = inner;
        }

        public virtual void EquipWeapon()
        {
            m_inner?.EquipWeapon();
        }

        public virtual void EquipHair()
        {
            m_inner?.EquipHair();
        }

        public virtual void EquipWing()
        {
            m_inner?.EquipWing();
        }

        public virtual void EquipTail()
        {
            m_inner?.EquipTail();
        }

        public virtual void EquipPant()
        {
            m_inner?.EquipPant();
        }

        public virtual void EquipSkin()
        {
            m_inner?.EquipSkin();
        }

        #endregion
    }

    public sealed class NullDecoratable : IDecoratable
    {
        #region -- Methods --

        public void EquipWeapon() { }
        public void EquipHair() { }
        public void EquipWing() { }
        public void EquipTail() { }
        public void EquipPant() { }
        public void EquipSkin() { }

        #endregion
    }

    [Serializable]
    public struct CustomVisualContext
    {
        #region -- Fields --

        public CustomData customData;

        public bool hasTextureInSkin;
        public Texture2D skinTexture;
        public Material skinMaterial;

        public Texture2D pantTexture;

        public GameObject weaponPrefab;
        public ProjectileBase projectilePrefab;
        public GameObject hairPrefab;
        public GameObject wingPrefab;
        public GameObject tailPrefab;

        #endregion
    }
}