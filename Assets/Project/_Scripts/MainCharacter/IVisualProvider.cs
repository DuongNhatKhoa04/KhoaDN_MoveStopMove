using MoveStopMove.Weapon.Projectile;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public interface IVisualProvider
    {
       public Material DefaultSkinMaterial { get; }

       public GameObject GetWeaponPrefabFromData(string weapon);
       public ProjectileBase GetProjectileFromData(string weapon);
       public GameObject GetHairPrefabFromData(string hair);
       public GameObject GetWingPrefabFromData();
       public GameObject GetTailPrefabFromData();
       public Texture2D GetPantTextureFromData(string pant);
    }
}