using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class PantDecorator : CharacterDecorator
    {
        private Texture2D m_albedoTexture;
        public int MaterialIndex { private get; set; } = 0;
        //public Texture2D AlbedoTexture { get; set; }

        // Shader property IDs
        private static readonly int _MainTex      = Shader.PropertyToID("_MainTex"); // Built-in/Standard
        private static readonly int _BaseMap      = Shader.PropertyToID("_BaseMap"); // URP Lit
        private static readonly int _BaseColorMap = Shader.PropertyToID("_BaseColorMap"); // HDRP Lit

        public PantDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("PantDecorator");
        }

        public override void EquipPant()
        {
            base.EquipPant();

            if (!PantTexture)
            {
                Debug.Log("texture null");
                return;
            }

            var mats = PantsRenderer.materials;
            if (MaterialIndex < 0 || MaterialIndex >= mats.Length)
            {
                Debug.Log($"rdr null: {PantsRenderer == null}, tex null: {PantTexture == null}");
                return;
            }

            var mat = mats[MaterialIndex];

            if (mat.HasProperty(_BaseMap))
                mat.SetTexture(_BaseMap, PantTexture); // URP
            else if (mat.HasProperty(_BaseColorMap))
                mat.SetTexture(_BaseColorMap, PantTexture); // HDRP
            else
                mat.SetTexture(_MainTex, PantTexture); // Standard

            mats[MaterialIndex] = mat;
            PantsRenderer.materials = mats;
        }
    }
}