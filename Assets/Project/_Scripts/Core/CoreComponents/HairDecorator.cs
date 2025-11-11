using MoveStopMove.Interfaces;

namespace MoveStopMove.Core.CoreComponents
{
    public class HairDecorator : CharacterDecorator
    {
        public HairDecorator(IDecoratable inner) : base(inner) { }


    }
}