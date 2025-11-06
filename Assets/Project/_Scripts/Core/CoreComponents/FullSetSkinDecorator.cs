using MoveStopMove.Interfaces;

namespace MoveStopMove.Core.CoreComponents
{
    public class FullSetSkinDecorator : CharacterDecorator
    {
        public FullSetSkinDecorator(IDecoratable inner)
            : base(inner) { }
    }
}