using UnityEngine;

namespace MoveStopMove.Interfaces
{
    public interface IMoveable
    {
        public bool IsGrounded();
        public void Movement(Vector3 direction, float speed);
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
}