using MoveStopMove.Core;
using MoveStopMove.Extensions.Observer;
using MoveStopMove.Managers;
using UnityEngine;

namespace MoveStopMove.Bot
{
    public class Enemy : Character, IMyObserver<HitTarget>
    {
        private void OnEnable()
        {
            EventManager.Instance?.Subscribe<HitTarget>(this);
        }

        private void OnDisable()
        {
            EventManager.Instance?.Unsubscribe<HitTarget>(this);
        }

        public void OnNotify(HitTarget data)
        {
            if (data.Target == gameObject)
            {
                Debug.Log("Defeated by " + data.Victim);
                CharacterPool.Release(this);
            }
        }
    }
}