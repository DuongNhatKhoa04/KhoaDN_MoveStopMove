namespace MoveStopMove.Extensions.Observer
{
    public interface IMyObserver<in T>
    {
        public void OnNotify(T data);
    }
}