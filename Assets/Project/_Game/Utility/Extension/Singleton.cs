using UnityEngine;

namespace MoveStopMove.Utility.Extension
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region -- Fields --

        private static T s_instance;
        [SerializeField] protected bool dontDestroyOnLoad = true;

        #endregion

        #region -- Properties --

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();

                    if (s_instance == null)
                    {
                        SetUpInstance();
                    }
                }
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
            GameObject singleton = new GameObject(typeof(T).Name);
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

        /// <summary>
        /// Remove duplicate instances
        /// </summary>
        private void RemoveDuplicates()
        {
            if (s_instance == null)
            {
                s_instance = this as T;

                if (dontDestroyOnLoad)
                {
                    var root = transform.root;

                    if (root != transform)
                        DontDestroyOnLoad(root);
                    else
                        DontDestroyOnLoad(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        #endregion
    }
}