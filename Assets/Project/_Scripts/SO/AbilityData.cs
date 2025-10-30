using Sirenix.OdinInspector;
using UnityEngine;

namespace MoveStopMove.SO
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "MoveStopMove/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        [HorizontalGroup("Row1", 320)]
        [VerticalGroup("Row1/Left")] public AnimationClip animationClip;
        [VerticalGroup("Row1/Left"), ReadOnly] public int animationHash;
        [VerticalGroup("Row1/Left")] public float duration;
        [PreviewField(60, ObjectFieldAlignment.Right)]
        [VerticalGroup("Row1/Right"), HideLabel] public Sprite icon;

        private void OnValidate()
        {
            animationHash = Animator.StringToHash(animationClip.name);
        }
    }
}