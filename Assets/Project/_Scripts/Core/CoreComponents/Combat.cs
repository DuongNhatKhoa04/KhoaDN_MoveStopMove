using System;
using System.Collections.Generic;
using MoveStopMove.Extensions.ObjectPooling;
using MoveStopMove.Extensions.Observer;
using MoveStopMove.Managers;
using MoveStopMove.Weapon;
using UnityEngine;
using UnityEngine.Pool;
using MoveStopMove.Weapon.Projectile;

namespace MoveStopMove.Core.CoreComponents
{
    public class Combat : CoreComponents
    {
        [SerializeField] private AttackRange attackRange;
        [SerializeField] private WeaponBase weapon;

        [Header("Projectile Pool Settings")]
        [SerializeField] private int poolMaxSize = 50;

        [Header("Projectile Settings")]
        [SerializeField] private ProjectileBase projectilePrefab;
        [SerializeField] private Transform weaponSpawnPoint;
        [SerializeField] private float projectileSpeed = 20f;
        [SerializeField] private float safeSpawnDistance = 1.0f;

        private IObjectPool<ProjectileBase> m_projectilePool;

        private void Awake()
        {
            // Khởi tạo pool nếu Weapon có sử dụng projectile
            if (weapon != null && weapon is WeaponBase normalWeapon)
            {
                m_projectilePool = new ObjectPool<ProjectileBase>(
                    weapon.ProjectilePooling.CreateProjectile,
                    weapon.ProjectilePooling.OnGetProjectile,
                    weapon.ProjectilePooling.OnReleaseProjectile,
                    weapon.ProjectilePooling.OnDestroyProjectile,
                    maxSize: poolMaxSize
                );
                weapon.ProjectilePooling.SetPool(m_projectilePool);
            }
        }

        private void Update()
        {
            // Giả lập input — nhấn Space để tấn công mục tiêu đầu tiên
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var targetEntry = attackRange.PeekEntry();
                if (targetEntry == null)
                {
                    Debug.Log("Không có kẻ địch trong vùng tấn công!");
                    return;
                }

                var target = targetEntry.Value.Target;
                Vector3 targetPosition = AttackRange.GetTargetPosition(targetEntry.Value);

                Debug.Log($"Tấn công {target.name} bằng {weapon.GetActiveWeaponMode()}");

                // Gọi tấn công thông qua Weapon
                weapon.Attack(targetPosition);
            }
        }
    }
}