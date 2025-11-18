namespace MoveStopMove.Core.Interfaces
{
    public interface IMyObserver<in T>
    {
        public void OnNotify(T data);
    }
}