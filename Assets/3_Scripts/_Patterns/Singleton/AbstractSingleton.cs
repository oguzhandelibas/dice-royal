using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ODProjects
{
    /// <summary>
    /// An abstract class that provides base functionalities of a singleton for its derived classes
    /// </summary>
    /// <typeparam name="T">The type of singleton instance</typeparam>
    public abstract class AbstractSingleton<T> : MonoBehaviour where T : Component
    {
        static T s_Instance;

        /// <summary>
        /// static Singleton instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<T>();
                    if (s_Instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        GameObject singletonObject = GameObject.Find("--->Singletons");
                        if (singletonObject == null) singletonObject = new GameObject("--->Singletons");
                        obj.transform.SetParent(singletonObject.transform);
                        s_Instance = obj.AddComponent<T>();
                    }
                }

                return s_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}