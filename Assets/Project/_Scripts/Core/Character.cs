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

        public MainCore Core => core;
        public FiniteStateMachine StateMachine { get; private set; }
        public PlayerIdleState CharacterIdleState { get; private set; }
        public MoveState CharacterMoveState { get; private set; }
        public AttackState CharacterAttackState {  get; private set; }

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
        private IDecoratable m_decoratable;
        public SkinnedMeshRenderer PantsRenderer { get; set; }
        public SkinnedMeshRenderer SkinSetRenderer { get; set; }
        public Texture2D PantTexture { get; set; }
        public Texture2D SkinSetTexture { get; set; }

        private static readonly int s_mainTex = Shader.PropertyToID("_MainTex");

        protected CharacterDecorator(IDecoratable inner)
        {
            Debug.Log("CharacterDecorator");
            m_decoratable = inner;
        }

        public virtual void EquipWeapon()
        {
            m_decoratable.EquipWeapon();
        }

        public virtual void EquipPant()
        {
            m_decoratable.EquipPant();
            SetAlbedoForMaterial(PantsRenderer,PantTexture);
        }

        public void EquipSkinSet()
        {
            m_decoratable.EquipSkinSet();
            SetAlbedoForMaterial(SkinSetRenderer,SkinSetTexture);
        }

        private void SetAlbedoForMaterial(SkinnedMeshRenderer skinMesh,Texture2D texture)
        {
            if (!skinMesh)
            {
                Debug.LogWarning("SetAlbedoForMaterial: SkinnedMeshRenderer bị null!");
                return;
            }

            if (!texture)
            {
                Debug.LogWarning($"SetAlbedoForMaterial: Texture null trên {skinMesh.name}");
                return;
            }

            var materialArray = skinMesh.materials;
            if (materialArray.Length == 0)
            {
                Debug.LogWarning($"SetAlbedoForMaterial: {skinMesh.name} không có materials!");
                return;
            }

            var material = materialArray[0];
            material.SetTexture(s_mainTex, texture);
            skinMesh.materials = materialArray;
        }
    }

    public sealed class NullDecoratable : IDecoratable
    {
        public void EquipWeapon() { }
        public void EquipPant() { }
        public void EquipSkinSet() { }
    }
}