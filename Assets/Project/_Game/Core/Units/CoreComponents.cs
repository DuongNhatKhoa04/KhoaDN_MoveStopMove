using UnityEngine;

namespace MoveStopMove.Core.Units
{
    public abstract class CoreComponents : MonoBehaviour
    {
        #region -- Fields --

        protected MainCore Core;

        #endregion

        #region -- Methods --

        protected virtual void Awake()
        {
            Core = GetComponentInParent<MainCore>();

            if (Core == null)
            {
                Debug.LogError("There is no Core on the parent");
            }
        }

        #endregion
    }
}