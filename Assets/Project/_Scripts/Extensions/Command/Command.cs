namespace MoveStopMove.Extensions.Command
{
    public abstract class Command
    {
        #region -- Fields --

        public object Data;

        #endregion

        #region -- Methods --

        public abstract void Execute();
        public abstract void Undo();

        #endregion
    }

    public class CommandNode
    {
        #region -- Fields --

        public string Name;
        public object Data;

        #endregion

        #region -- Methods --

        public CommandNode(string name, object data)
        {
            Name = name;
            Data = data;
        }

        #endregion
    }
}