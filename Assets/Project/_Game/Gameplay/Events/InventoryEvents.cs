namespace MoveStopMove.Gameplay.Events
{
    public enum EItemAction
    {
        Equip,
        Unequip
    }

    public readonly struct ItemEvent
    {
        public readonly EItemAction Action;
        public readonly string Item;

        public ItemEvent(EItemAction action, string item)
        {
            Action = action;
            Item = item;
        }
    }
}