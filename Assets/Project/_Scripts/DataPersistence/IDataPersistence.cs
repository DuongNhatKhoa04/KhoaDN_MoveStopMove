using MoveStopMove.DataPersistence.Data;

namespace MoveStopMove.DataPersistence
{
    public interface IDataPersistence
    {
        public void LoadData(GameData data);

        public void SaveData(GameData data);
    }
}