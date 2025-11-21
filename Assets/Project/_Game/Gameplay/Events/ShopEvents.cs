namespace MoveStopMove.Gameplay.Events
{
    public readonly struct ShopBuyEvent
    {
        public readonly string Item;
        public readonly int Price;

        public ShopBuyEvent(string item, int price)
        {
            Item = item;
            Price = price;
        }
    }
}