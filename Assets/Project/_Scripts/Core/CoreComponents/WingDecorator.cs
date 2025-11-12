using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class WingDecorator : CharacterDecorator
    {
        public WingDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("WingDecorator");
        }
    }
}