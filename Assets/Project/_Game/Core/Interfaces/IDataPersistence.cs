using MoveStopMove.Core.SaveLoad.Data;

namespace MoveStopMove.Core.Interfaces
{
    public interface IDataPersistence
    {
        public void LoadData(GameData data);
        public void SaveData(GameData data);
    }
}