using System.Collections;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.Stats;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Gameplay.Projectiles
{
    public class ReturnableProjectile : ProjectileBase
    {
        #region -- Fields --

        [Header("Return Settings")]
        [SerializeField] private float forwardDistance   = 10f;
        [SerializeField] private float heightOffset      = 1f;
        [SerializeField] private float delayBeforeReturn = 1f;
        [SerializeField] private float catchDistance     = 1f;

        private Transform m_player;
        private bool m_goingOut;
        private Vector3 m_forwardTarget;
        private Coroutine m_returnRoutine;

        private Vector3 m_baseEuler;
        private float m_spinAngle;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            m_baseEuler = transform.eulerAngles;
        }

        public override void Initialize(GameObject attacker, Vector3 targetPos)
        {
            base.Initialize(attacker, targetPos);

            m_player = attacker != null ? attacker.transform : null;
            if (m_player != null)
            {
                Vector3 origin = m_player.position + Vector3.up * heightOffset;
                transform.position = origin;

                m_forwardTarget = origin + m_player.forward * forwardDistance;
            }

            m_goingOut = true;

            m_spinAngle = 0f;

            if (m_returnRoutine != null)
            {
                StopCoroutine(m_returnRoutine);
            }

            m_returnRoutine = StartCoroutine(ReturnDelayCoroutine());

            transform.eulerAngles = m_baseEuler;
        }

        private IEnumerator ReturnDelayCoroutine()
        {
            yield return new WaitForSeconds(delayBeforeReturn);
            m_goingOut = false;
        }

        protected override void Update()
        {
            if (m_player == null)
            {
                ReturnToPool();
                return;
            }

            m_spinAngle += 500f * Time.deltaTime;
            float y = m_baseEuler.y + m_spinAngle;
            transform.eulerAngles = new Vector3(m_baseEuler.x, y, m_baseEuler.z);

            Vector3 targetPos = m_goingOut
                ? m_forwardTarget
                : m_player.position + Vector3.up * heightOffset;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            if (!m_goingOut && Vector3.Distance(transform.position, targetPos) <= catchDistance)
            {
                ReturnToPool();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (m_returnRoutine != null)
            {
                StopCoroutine(m_returnRoutine);
                m_returnRoutine = null;
            }

            m_player   = null;
            m_goingOut = false;
        }

        protected override void OnHitTarget(GameObject target)
        {
            if (target == Owner)
                return;

            if ((hittableLayers.value & (1 << target.layer)) == 0)
                return;

            EventManager.Instance.Notify(new HitTarget(Owner, target, 1));

            //ReturnToPool();
        }

        #endregion
    }
}