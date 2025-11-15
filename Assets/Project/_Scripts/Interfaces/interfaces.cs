using MoveStopMove.Core;
using UnityEngine;

namespace MoveStopMove.Interfaces
{
    public interface IMoveable
    {
        public bool IsGrounded();
        public void Moving(Vector3 direction, float speed, float acceleration);
        public void Stop();
    }

    public interface IDamageable
    {
        public void TakeDamage();
        public void ApplyDamage();
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
}