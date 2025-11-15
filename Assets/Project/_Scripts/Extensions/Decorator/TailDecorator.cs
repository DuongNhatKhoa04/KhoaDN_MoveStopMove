using MoveStopMove.Core;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
{
    public class TailDecorator : CharacterDecorator
    {
        private GameObject m_currentTail;
        public GameObject TailAttachment { get; set; }
        public GameObject TailPrefab { get; set; }

        public TailDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("TailDecorator");
        }

        public override void EquipTail()
        {
            base.EquipTail();
            Debug.Log(TailPrefab?.name);

            if (TailAttachment == null)
            {
                Debug.Log("TailAttachment is not assigned");
                return;
            }

            if (m_currentTail != null)
            {
                Object.Destroy(m_currentTail);
                m_currentTail = null;
            }

            if (TailPrefab != null)
            {
                m_currentTail = Object.Instantiate(TailPrefab, TailAttachment.transform);
            }
        }
    }
}