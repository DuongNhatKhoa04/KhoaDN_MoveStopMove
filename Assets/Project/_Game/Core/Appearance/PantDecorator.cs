using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.Appearance
{
    public class PantDecorator : CharacterDecorator
    {
        #region -- Methods --

        public PantDecorator(IDecoratable inner) : base(inner)
        {
            //Debug.Log("PantDecorator");
        }

        /// <summary>
        /// Equip pant by change texture
        /// </summary>
        public override void EquipPant()
        {
            base.EquipPant();
            PlayerSaveLoader.SetAlbedoForMaterial(PantsRenderer, PantTexture);
        }

        #endregion
    }
}