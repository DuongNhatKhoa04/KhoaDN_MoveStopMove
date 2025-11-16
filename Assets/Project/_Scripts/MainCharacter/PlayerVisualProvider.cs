using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;
using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public class PlayerVisualProvider : IVisualProvider
    {
        #region -- Fields --

        private readonly CustomVisualContext m_customContext;

        #endregion

        #region -- Properties --

        public Material DefaultSkinMaterial { get; }

        #endregion

        #region -- Methods --

        public PlayerVisualProvider(CustomVisualContext customContext, Material defaultSkinMaterial)
        {
            m_customContext = customContext;
            DefaultSkinMaterial = defaultSkinMaterial;
        }

        public GameObject GetWeaponPrefabFromData(string weapon)
        {
            return m_customContext.weaponPrefab ??
                   PlayerSaveLoader.GetDecoratorData<WeaponData, GameObject>(
                       weapon, PlayerSaveLoader.SO_WEAPON_PATH,
                       data => data.prefab);
        }

        public ProjectileBase GetProjectileFromData(string weapon)
        {
            return m_customContext.projectilePrefab ??
                   PlayerSaveLoader.GetDecoratorData<WeaponData, ProjectileBase>(
                       weapon, PlayerSaveLoader.SO_WEAPON_PATH,
                       data => data.projectilePrefab);
        }

        public GameObject GetHairPrefabFromData(string hair)
        {
            if (hair == "none") return null;

            return m_customContext.hairPrefab ??
                   PlayerSaveLoader.GetDecoratorData<HairData, GameObject>(
                       hair, PlayerSaveLoader.SO_HAIRS_PATH,
                       data => data.prefab);
        }

        public GameObject GetWingPrefabFromData() => m_customContext.wingPrefab;

        public GameObject GetTailPrefabFromData() => m_customContext.tailPrefab;

        public Texture2D GetPantTextureFromData(string pant)
        {
            if (pant == "none") return null;

            return m_customContext.pantTexture ??
                   PlayerSaveLoader.GetDecoratorData<PantData, Texture2D>(
                       pant, PlayerSaveLoader.SO_PANTS_PATH,
                       d => d.texture);
        }

        #endregion
    }
}