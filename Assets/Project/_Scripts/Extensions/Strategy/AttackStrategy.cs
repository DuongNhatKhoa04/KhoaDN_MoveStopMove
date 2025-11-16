using MoveStopMove.Weapon;
using UnityEngine;

namespace MoveStopMove.Extensions.Strategy
{
    public interface IAttackStrategy
    {
        public void PerformAttack(Vector3 targetPosition);
    }

    public class NormalAttackStrategy : IAttackStrategy
    {
        public void PerformAttack(Vector3 targetPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PiercingAttackStrategy : IAttackStrategy
    {
        private readonly PiercingWeapon m_weapon;

        public PiercingAttackStrategy(PiercingWeapon weapon)
        {
            m_weapon = weapon;
        }

        public void PerformAttack(Vector3 targetPosition)
        {
            m_weapon.SpawnPiercingProjectile(targetPosition);
        }
    }

    public class ChainableAttackStrategy : IAttackStrategy
    {
        public void PerformAttack(Vector3 targetPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ReturnableAttackStrategy : IAttackStrategy
    {
        public void PerformAttack(Vector3 targetPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}