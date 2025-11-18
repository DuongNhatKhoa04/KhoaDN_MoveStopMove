using MoveStopMove.Gameplay.Projectiles;
using UnityEngine;

namespace MoveStopMove.Core.Interfaces
{
    public interface IMoveable
    {
        public bool IsGrounded();
        public void Moving(Vector3 direction, float speed, float acceleration);
        public void Stop();
    }

    public interface IInitializable
    {
        public void Initialize();
    }

    public interface IPausable
    {
        public void Pause();
        public void Resume();
    }

    public interface IResettable
    {
        public void Reset();
    }

    public interface IDecoratable
    {
        public void EquipWeapon();
        public void EquipHair();
        public void EquipWing();
        public void EquipTail();
        public void EquipPant();
        public void EquipSkin();
    }

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