using UnityEngine;

namespace MoveStopMove.Gameplay.Items
{
    public abstract class ItemBase : MonoBehaviour
    {
        public string ItemName { get; protected set; }
        public int Price { get; protected set; }
        public Sprite Icon { get; protected set; }
        public string ItemBuff {  get; protected set; }

        public float RangeIncrease { get; protected set; }

        public float MovementIncrease { get; protected set; }

        public abstract void EquipItem();
        public abstract void UnequipItem();
        public abstract void BuyItem();
    }
}