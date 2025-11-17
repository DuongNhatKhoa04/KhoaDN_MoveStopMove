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
        private readonly NormalWeapon m_weapon;

        public NormalAttackStrategy(NormalWeapon weapon)
        {
            m_weapon = weapon;
        }

        public void PerformAttack(Vector3 targetPosition)
        {
            m_weapon.SpawnNormalProjectile(targetPosition);
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
        private readonly ChainableWeapon m_weapon;

        public ChainableAttackStrategy(ChainableWeapon weapon)
        {
            m_weapon = weapon;
        }

        public void PerformAttack(Vector3 targetPosition)
        {
            m_weapon.SpawnChainableProjectile(targetPosition);
        }
    }

    public class ReturnableAttackStrategy : IAttackStrategy
    {
        private readonly ReturnableWeapon m_weapon;

        public ReturnableAttackStrategy(ReturnableWeapon weapon)
        {
            m_weapon = weapon;
        }

        public void PerformAttack(Vector3 targetPosition)
        {
            m_weapon.SpawnReturnableProjectile(targetPosition);
        }
    }
}