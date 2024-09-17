﻿using UnityEngine;

namespace Singletons
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;
        
        public static bool HasInstance => instance != null;
        public static T TryGetInstance() => HasInstance ? instance : null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject($"{typeof(T).Name} Auto Generated");
                        instance = go.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;

            instance = this as T;
        }
    }
}