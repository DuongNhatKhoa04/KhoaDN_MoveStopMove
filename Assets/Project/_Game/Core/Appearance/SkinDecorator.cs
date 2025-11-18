using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.Appearance
{
    public class SkinDecorator : CharacterDecorator
    {
        #region -- Properties --

        public bool HasTexture { get; set; }
        public SkinnedMeshRenderer SkinSetRenderer { get; set; }
        public Texture2D SkinTexture { get; set; }
        public Material SkinMaterial { get; set; }
        public Material DefaultSkinMaterial { get; set; }

        #endregion

        #region -- Methods --

        public SkinDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("SkinDecorator");
        }

        /// <summary>
        /// Equip skin by change texture or material
        /// </summary>
        public override void EquipSkin()
        {
            base.EquipSkin();

            if (HasTexture)
            {
                PlayerSaveLoader.SetNewMaterialForSkin(SkinSetRenderer, DefaultSkinMaterial);
                PlayerSaveLoader.SetAlbedoForMaterial(SkinSetRenderer, SkinTexture);
            }
            else
            {
                PlayerSaveLoader.SetNewMaterialForSkin(SkinSetRenderer, SkinMaterial);
            }
        }

        #endregion
    }
}