using UnityEngine;
using UnityEngine.Pool;

namespace MoveStopMove.Gameplay.Projectiles
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        #region -- Fields --

        [Header("Projectile Settings")]
        [SerializeField] protected float speed = 10f;
        [SerializeField] private float maxLifetime = 5f;
        [SerializeField] private bool destroyOnHit = true;
        [SerializeField] protected LayerMask hittableLayers;

        public string weaponName;

        protected GameObject Owner;

        protected Vector3 Direction;
        private float m_lifetimeTimer;
        private bool m_active;

        private IObjectPool<ProjectileBase> m_objectPool;

        #endregion

        #region -- Properties --

        public IObjectPool<ProjectileBase> ObjectPool
        {
            set => m_objectPool = value;
        }

        #endregion

        #region -- Methods --

        protected virtual void Update()
        {
            if (!m_active) return;

            MoveForward();

            m_lifetimeTimer -= Time.deltaTime;
            if (m_lifetimeTimer <= 0f)
            {
                ReturnToPool();
            }
        }

        public virtual void Initialize(GameObject attacker, Vector3 targetPos)
        {
            Owner = attacker;
            m_lifetimeTimer = maxLifetime;
            m_active = true;

            gameObject.SetActive(true);

            Vector3 dir = targetPos - transform.position;
            if (dir.sqrMagnitude < 1e-4f)
            {
                dir = transform.forward;
            }
            Direction = dir.normalized;
            //transform.rotation = Quaternion.LookRotation(m_direction);

            if (Owner != null && Owner.TryGetComponent(out Collider ownerCol) && TryGetComponent(out Collider projCol))
            {
                Physics.IgnoreCollision(projCol, ownerCol, true);
            }
        }

        protected virtual void MoveForward()
        {
            transform.position += Direction * (speed * Time.deltaTime);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!m_active) return;

            var target = other.gameObject;

            if (target == Owner) return;

            OnHitTarget(target);
        }

        protected virtual void OnHitTarget(GameObject target)
        {
            if (destroyOnHit)
                ReturnToPool();
        }

        protected virtual void OnDisable()
        {
            Owner = null;
            m_active = false;
        }

        protected void ReturnToPool()
        {
            if (!m_active) return;

            m_active = false;
            m_lifetimeTimer = maxLifetime;

            if (m_objectPool != null)
                m_objectPool.Release(this);
            else
                gameObject.SetActive(false);
        }

        #endregion
    }
}