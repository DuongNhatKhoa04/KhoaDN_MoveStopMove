using MoveStopMove.Core;
using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Extensions.Decorator
{
    public class HairDecorator : CharacterDecorator
    {
        private GameObject m_currentHair;
        public GameObject HairAttachment { get; set; }
        public GameObject HairPrefab { get; set; }
        public HairDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("Hair decorator");
        }

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
    }
}