using MoveStopMove.Interfaces;

namespace MoveStopMove.Core.CoreComponents
{
    public class WeaponDecorator : CharacterDecorator
    {
        public WeaponDecorator(IDecoratable inner) : base(inner) { }
    }
}