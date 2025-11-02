using MoveStopMove.Extensions.Singleton;
using UnityEngine;

namespace MoveStopMove.Managers
{
    public class GamePlayManager : Singleton<GamePlayManager>
    {
        [SerializeField] private FixedJoystick fixedJoystick;
    }
}