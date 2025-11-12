using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class SkinDecorator : CharacterDecorator
    {
        public SkinDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("SkinDecorator");
        }

        public override void EquipSkin()
        {
            base.EquipSkin();
            SetNewMaterialForSkin(true);
        }
    }
}