using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
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
            PlayerSaveLoader.SetAlbedoForMaterial(PantsRenderer, PantTexture);
        }
    }
}