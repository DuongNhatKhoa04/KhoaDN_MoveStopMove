using MoveStopMove.Interfaces;
using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public class TailDecorator : CharacterDecorator
    {
        public TailDecorator(IDecoratable inner) : base(inner)
        {
            Debug.Log("TailDecorator");
        }
    }
}