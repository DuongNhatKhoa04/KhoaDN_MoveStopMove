using MoveStopMove.Core.CoreComponents;
using MoveStopMove.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Weapon
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float maxLifetime = 5f;
        [SerializeField] private bool destroyOnHit = true;

        protected GameObject Owner;
        private Vector3 m_targetPosition;
        private float m_lifetimeTimer;
        private bool m_active;

        private IObjectPool<ProjectileBase> m_pool;

        public void SetPool(IObjectPool<ProjectileBase> objectPool)
        {
            m_pool = objectPool;
        }

        public virtual void Initialize(GameObject attacker, Vector3 targetPos)
        {
            Owner = attacker;
            m_targetPosition = targetPos;
            m_lifetimeTimer = maxLifetime;
            m_active = true;

            gameObject.SetActive(true);
        }

        protected virtual void Update()
        {
            if (!m_active) return;

            MoveTowards(m_targetPosition);

            m_lifetimeTimer -= Time.deltaTime;
            if (m_lifetimeTimer <= 0f)
            {
                ReturnToPool();
            }
        }

        protected virtual void MoveTowards(Vector3 destination)
        {
            Vector3 direction = (destination - transform.position).normalized;
            transform.position += direction * (speed * Time.deltaTime);
            transform.forward = direction;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!m_active) return;
            if (other.attachedRigidbody == null) return;

            var target = other.attachedRigidbody.gameObject;

            if (target == Owner) return;

            OnHitTarget(target);
        }

        protected virtual void OnHitTarget(GameObject target)
        {
            if (destroyOnHit)
                ReturnToPool();
        }

        protected void ReturnToPool()
        {
            if (!m_active) return;
            m_active = false;

            m_lifetimeTimer = maxLifetime;

            if (m_pool != null)
            {
                m_pool.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
            Owner = null;
            m_active = false;
        }
    }
}
