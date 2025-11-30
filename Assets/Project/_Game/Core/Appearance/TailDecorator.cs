using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using UnityEngine;

namespace MoveStopMove.Core.Appearance
{
    public class TailDecorator : CharacterDecorator
    {
        #region -- Fields --

        private GameObject m_currentTail;

        #endregion

        #region -- Properties --

        public GameObject TailAttachment { get; set; }
        public GameObject TailPrefab { get; set; }

        #endregion

        #region -- Methods --

        public TailDecorator(IDecoratable inner) : base(inner) { }

        /// <summary>
        /// Equip tail to the tail attachment
        /// </summary>
        public override void EquipTail()
        {
            base.EquipTail();

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

        #endregion
    }
}