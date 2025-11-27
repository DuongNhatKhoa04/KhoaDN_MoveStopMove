using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.SaveLoad.Data;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.Shops
{
    public class ShopManager : Singleton<ShopManager>
    {
        private GameData m_gameData;

        public GameData GameData
        {
            get => m_gameData;
            set => m_gameData = value;
        }

        private void Start()
        {
            m_gameData = DataPersistenceManager.Instance.GameData;
        }

        public void BuyWeapon(string weaponName, float price)
        {
            if (m_gameData.coins < price)
            {
                EventManager.Instance.Notify(new NotificationPopUpEvent(EEventCode.NotEnoughCoins));
            }
            else
            {

            }
        }
    }
}