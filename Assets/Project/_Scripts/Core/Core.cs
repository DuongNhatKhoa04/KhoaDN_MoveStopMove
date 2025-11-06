using MoveStopMove.Core.CoreComponents;
using MoveStopMove.Extensions.FSM.States;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class MainCore : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private Combat combat;

        public Movement Movement => movement;
        public Combat Combat => combat;
    }
}