using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class PantDecorator : CharacterDecorator
    {
        public PantDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("PantDecorator");
        }

        public override void EquipPant()
        {
            base.EquipPant();
            SetAlbedoForMaterial(PantsRenderer,PantTexture);
        }
    }
}