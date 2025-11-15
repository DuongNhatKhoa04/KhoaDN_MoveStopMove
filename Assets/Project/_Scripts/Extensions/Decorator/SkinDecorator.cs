using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
{
    public class SkinDecorator : CharacterDecorator
    {
        public bool HasTexture { get; set; }
        public SkinnedMeshRenderer SkinSetRenderer { get; set; }
        public Texture2D SkinTexture { get; set; }
        public Material SkinMaterial { get; set; }
        public Material DefaultSkinMaterial { get; set; }

        public SkinDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("SkinDecorator");
        }

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
    }
}