using UnityEngine;

namespace MoveStopMove.Core.CoreComponents
{
    public abstract class CoreComponents : MonoBehaviour
    {
        protected MainCore Core;

        protected virtual void Awake()
        {
            Core = GetComponentInParent<MainCore>();

            if (Core == null)
            {
                Debug.LogError("There is no Core on the parent");
            }
        }
    }
}