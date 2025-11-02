using Sirenix.OdinInspector;
using UnityEngine;

namespace MoveStopMove.SO
{
    [CreateAssetMenu(fileName = "AnimationData", menuName = "MoveStopMove/AnimationData")]
    public class AnimationData : ScriptableObject
    {
        public AnimationClip animationClip;
        public int animationHash;
        public int speed;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animationClip.name);
        }
    }
}