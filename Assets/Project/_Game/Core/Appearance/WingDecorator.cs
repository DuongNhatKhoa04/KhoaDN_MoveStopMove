using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using UnityEngine;

namespace MoveStopMove.Core.Appearance
{
    public class WingDecorator : CharacterDecorator
    {
        #region -- Fields --

        private GameObject m_currentWing;

        #endregion

        #region -- Properties --

        public GameObject WingAttachment { get; set; }
        public GameObject WingPrefab { get; set; }

        #endregion

        #region -- Methods --

        public WingDecorator(IDecoratable inner) : base(inner) { }

        /// <summary>
        /// Equip wing to wing attachment
        /// </summary>
        public override void EquipWing()
        {
            base.EquipWing();

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

        #endregion
    }
}