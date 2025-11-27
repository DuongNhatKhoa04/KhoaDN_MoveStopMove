using System;
using System.Collections.Generic;
using MoveStopMove.Utility.Extension;
using UnityEngine;
using UnityEngine.Events;

namespace MoveStopMove.Presentation.UI
{
    public class UIManager : Singleton<UIManager>
    {
        #region -- Fields --

        //dict for quick query UI prefab
        private Dictionary<Type, UICanvas> m_uiCanvasPrefab = new();
        //list from resource
        private UICanvas[] m_uiResources;
        //dict for UI active
        private Dictionary<Type, UICanvas> m_uiCanvas = new();
        private Dictionary<UICanvas, UnityAction> m_backActionEvents = new();
        private List<UICanvas> m_backCanvas = new();

        public Transform canvasParentTransform;

        #endregion

        #region -- Methods --

        #region - Canvas -

        private void Awake()
        {
            base.Awake();

            var noti = GetUI<UINotification>();

            noti.gameObject.SetActive(false);
        }

        public T OpenUI<T>() where T : UICanvas
        {
            UICanvas canvas = GetUI<T>();

            canvas.Setup();
            canvas.Open();

            return canvas as T;
        }

        public void CloseUI<T>() where T : UICanvas
        {
            if (IsOpened<T>())
            {
                GetUI<T>().Close();
            }
        }

        public bool IsOpened<T>() where T : UICanvas
        {
            return IsLoaded<T>() && m_uiCanvas[typeof(T)].gameObject.activeInHierarchy;
        }


        public bool IsLoaded<T>() where T : UICanvas
        {
            Type type = typeof(T);
            return m_uiCanvas.ContainsKey(type) && m_uiCanvas[type] != null;
        }

        public T GetUI<T>() where T : UICanvas
        {
            if (!IsLoaded<T>())
            {
                UICanvas canvas = Instantiate(GetUIPrefab<T>(), canvasParentTransform);
                m_uiCanvas[typeof(T)] = canvas;
            }

            return m_uiCanvas[typeof(T)] as T;
        }


        private T GetUIPrefab<T>() where T : UICanvas
        {
            if (!m_uiCanvasPrefab.ContainsKey(typeof(T)))
            {
                if (m_uiResources == null)
                {
                    m_uiResources = Resources.LoadAll<UICanvas>("UI/");
                    Debug.Log($"[UIManager] Loaded {m_uiResources.Length} UI prefabs from Resources/UI/");

                    foreach (var ui in m_uiResources)
                    {
                        Debug.Log($"[UIManager] Found UI prefab: {ui.name}");
                    }
                }

                for (int i = 0; i < m_uiResources.Length; i++)
                {
                    if (m_uiResources[i] is T)
                    {
                        m_uiCanvasPrefab[typeof(T)] = m_uiResources[i];
                        break;
                    }
                }
            }

            return m_uiCanvasPrefab[typeof(T)] as T;
        }


        #endregion

        #region  - Back Button -

        private UICanvas BackTopUI
        {
            get
            {
                UICanvas canvas = null;
                if (m_backCanvas.Count > 0)
                {
                    canvas = m_backCanvas[m_backCanvas.Count - 1];
                }

                return canvas;
            }
        }


        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
            {
                m_backActionEvents[BackTopUI]?.Invoke();
            }
        }

        public void PushBackAction(UICanvas canvas, UnityAction action)
        {
            if (!m_backActionEvents.ContainsKey(canvas))
            {
                m_backActionEvents.Add(canvas, action);
            }
        }

        public void AddBackUI(UICanvas canvas)
        {
            if (!m_backCanvas.Contains(canvas))
            {
                m_backCanvas.Add(canvas);
            }
        }

        public void RemoveBackUI(UICanvas canvas)
        {
            m_backCanvas.Remove(canvas);
        }

        /// <summary>
        /// CLear backey when comeback index UI canvas
        /// </summary>
        public void ClearBackKey()
        {
            m_backCanvas.Clear();
        }

        #endregion

        #endregion
    }
}