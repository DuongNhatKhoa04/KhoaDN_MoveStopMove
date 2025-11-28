using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.Units;
using UnityEngine;

namespace MoveStopMove.Core.Appearance
{
    public class HairDecorator : CharacterDecorator
    {
        #region -- Fields --

        private GameObject m_currentHair;

        #endregion

        #region -- Properties --

        public GameObject HairAttachment { get; set; }
        public GameObject HairPrefab { get; set; }

        #endregion

        #region -- Methods --

        public HairDecorator(IDecoratable inner) : base(inner)
        {
            //Debug.Log("Hair decorator");
        }

        /// <summary>
        /// Equip hair to hair attachment
        /// </summary>
        public override void EquipHair()
        {
            base.EquipHair();
            //Debug.Log(HairPrefab.name);

            if (HairAttachment == null)
            {
                Debug.Log("HairAttachment is not assigned");
                return;
            }

            if (m_currentHair != null)
            {
                Object.Destroy(m_currentHair);
                m_currentHair = null;
            }

            if (HairPrefab != null)
            {
                m_currentHair = Object.Instantiate(HairPrefab, HairAttachment.transform);
            }
        }

        #endregion
    }
}