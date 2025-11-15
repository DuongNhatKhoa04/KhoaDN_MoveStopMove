using MoveStopMove.Core;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
{
    public class WingDecorator : CharacterDecorator
    {
        private GameObject m_currentWing;
        public GameObject WingAttachment { get; set; }
        public GameObject WingPrefab { get; set; }
        public WingDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("WingDecorator");
        }

        public override void EquipWing()
        {
            base.EquipWing();
            Debug.Log(WingPrefab?.name);
            
            if (WingAttachment == null)
            {
                Debug.Log("WingAttachment is not assigned");
                return;
            }

            if (m_currentWing != null)
            {
                Object.Destroy(m_currentWing);
                m_currentWing = null;
            }

            if (WingPrefab != null)
            {
                m_currentWing = Object.Instantiate(WingPrefab, WingAttachment.transform);
            }
        }
    }
}