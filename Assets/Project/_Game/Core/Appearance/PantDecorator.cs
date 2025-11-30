using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.Appearance
{
    public class PantDecorator : CharacterDecorator
    {
        #region -- Methods --

        public PantDecorator(IDecoratable inner) : base(inner) { }

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