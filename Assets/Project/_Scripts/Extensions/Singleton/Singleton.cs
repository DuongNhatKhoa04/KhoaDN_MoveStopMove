using UnityEngine;

namespace MoveStopMove.Extensions.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;
        [SerializeField] protected bool dontDestroyOnLoad = true;

        public static T Instance
        {
            get
            {
                if (s_instance != null) return s_instance;

                s_instance = FindFirstObjectByType<T>();

                if (s_instance != null) return s_instance;

                SetUpInstance();

                return s_instance;
            }
        }

        private static void SetUpInstance()
        {
            if (s_instance != null) return;

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
    }
}