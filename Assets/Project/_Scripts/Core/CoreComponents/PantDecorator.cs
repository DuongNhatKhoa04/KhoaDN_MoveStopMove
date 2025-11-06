using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class PantDecorator : CharacterDecorator
    {
        public SkinnedMeshRenderer PantsRenderer { private get; set; }
        public int MaterialIndex { private get; set; } = 0;
        public Texture2D AlbedoTexture { private get; set; }

        // Shader property IDs
        private static readonly int _MainTex      = Shader.PropertyToID("_MainTex");        // Built-in/Standard
        private static readonly int _BaseMap      = Shader.PropertyToID("_BaseMap");        // URP Lit
        private static readonly int _BaseColorMap = Shader.PropertyToID("_BaseColorMap");   // HDRP Lit

        public PantDecorator(IDecoratable inner) : base(inner) { }

        public override void EquipPant()
        {
            // gọi inner trước (nếu có decorator khác trong chain)
            base.EquipPant();

            if (!PantsRenderer || !AlbedoTexture) return;

            var mats = PantsRenderer.materials;                 // instance materials (không phá asset gốc)
            if (MaterialIndex < 0 || MaterialIndex >= mats.Length) return;

            var mat = mats[MaterialIndex];

            if      (mat.HasProperty(_BaseMap))      mat.SetTexture(_BaseMap, AlbedoTexture);       // URP
            else if (mat.HasProperty(_BaseColorMap)) mat.SetTexture(_BaseColorMap, AlbedoTexture);  // HDRP
            else                                      mat.SetTexture(_MainTex, AlbedoTexture);      // Standard

            mats[MaterialIndex] = mat;
            PantsRenderer.materials = mats;
        }
    }
}