using System;
using MoveStopMove.Extensions.FSM;
using MoveStopMove.Extensions.FSM.States;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using MoveStopMove.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Core
{
    public abstract class Character : MonoBehaviour, IInitializable
    {
        #region -- Fields --

        [Header("Character Animation")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected EAnim currentAnimation;
        [SerializeField] protected MainCore core;
        [SerializeField] protected CharacterData characterData;

        #endregion

        #region -- Properties --

        public MainCore Core => core;
        public FiniteStateMachine StateMachine { get; private set; }
        public PlayerIdleState CharacterIdleState { get; private set; }
        public MoveState CharacterMoveState { get; private set; }
        public AttackState CharacterAttackState {  get; private set; }

        #endregion

        #region -- Methods --

        public virtual void Initialize()
        {
            InitAttackRange(characterData.attackRangeRadius);
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
            InitStateMachine();
        }

        private void InitAttackRange(float initRange)
        {
            core.Combat.GetAttackRange.InitRange(initRange);
        }

        protected void UpdateRange(float rangeIncrease)
        {
            core.Combat.GetAttackRange.InitRange(rangeIncrease);
        }

        private void InitStateMachine()
        {
            StateMachine = new FiniteStateMachine();

            CharacterIdleState = new PlayerIdleState(this, StateMachine, characterData, EAnim.Idle);
            CharacterMoveState = new MoveState(this, StateMachine, characterData, EAnim.Run);
            CharacterAttackState = new AttackState(this, StateMachine, characterData, EAnim.Attack);
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

        #endregion
    }

    public abstract class CharacterDecorator : IDecoratable
    {
        #region -- Fields --

        private IDecoratable m_inner;
        private static readonly int s_mainTex = Shader.PropertyToID("_MainTex");

        #endregion

        #region -- Properties --

        public bool HasHairInSkin { get; set; }
        public bool HasPantInSkin { get; set; }
        public bool HasTailInSkin { get; set; }
        public bool HasWingInSkin { get; set; }
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
            m_inner.EquipWeapon();
        }

        public virtual void EquipHair()
        {
            m_inner.EquipHair();
        }

        public virtual void EquipWing()
        {
            m_inner.EquipWing();
        }

        public virtual void EquipTail()
        {
            m_inner.EquipTail();
        }

        public virtual void EquipPant()
        {
            m_inner.EquipPant();
        }

        public virtual void EquipSkin()
        {
            m_inner.EquipSkin();
        }

        #endregion
    }

    public sealed class NullDecoratable : IDecoratable
    {
        public void EquipWeapon() { }
        public void EquipHair() { }
        public void EquipWing() { }
        public void EquipTail() { }
        public void EquipPant() { }
        public void EquipSkin() { }
    }

    [Serializable]
    public struct CustomVisualContext
    {
        public CustomData customData;

        public bool hasTextureInSkin;
        public Texture2D skinTexture;
        public Material skinMaterial;

        public Texture2D pantTexture;

        public GameObject weaponPrefab;
        public GameObject hairPrefab;
        public GameObject wingPrefab;
        public GameObject tailPrefab;
    }
}