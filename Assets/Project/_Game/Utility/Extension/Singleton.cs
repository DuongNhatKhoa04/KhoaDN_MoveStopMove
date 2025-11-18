using UnityEngine;

namespace MoveStopMove.Utility.Extension
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region -- Fields --

        private static T s_instance;
        private static bool s_isQuitting;
        [SerializeField] protected bool dontDestroyOnLoad = true;

        #endregion

        #region -- Properties --

        public static T Instance
        {
            get
            {
                if (s_isQuitting)
                    return null;

                if (s_instance != null) return s_instance;

                s_instance = FindFirstObjectByType<T>();

                if (s_instance != null) return s_instance;

                SetUpInstance();

                return s_instance;
            }
        }

        #endregion

        #region -- Methods --

        /// <summary>
        /// Setup instance of object
        /// </summary>
        private static void SetUpInstance()
        {
            if (s_instance != null  || s_isQuitting) return;

            var singleton = new GameObject(typeof(T).Name);
            s_instance = singleton.AddComponent<T>();

            DontDestroyOnLoad(singleton);
        }

        protected virtual void Awake()
        {
            RemoveDuplicates();
        }

        protected virtual void OnDestroy()
        {
            if (s_instance == this) s_instance = null;
        }

        protected virtual void OnApplicationQuit()
        {
            s_isQuitting = true;
        }

        /// <summary>
        /// Remove duplicate instances
        /// </summary>
        private void RemoveDuplicates()
        {
            if (s_instance == null)
            {
                s_instance = this as T;

                if (!dontDestroyOnLoad) return;

                var root = transform.root;

                if (root != transform)
                    DontDestroyOnLoad(root);
                else
                    DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }

        #endregion
    }
}