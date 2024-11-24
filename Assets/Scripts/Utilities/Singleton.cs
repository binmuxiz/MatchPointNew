using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance of {typeof(T)} is already destroyed. Returning null..");
                return null;
            }

            // 멀티스레드 환경에서 동시 접근 제어
            lock (_lock)
            {
                if (_instance == null)
                {
                    // 씬에서 인스턴스 검색
                    _instance = FindAnyObjectByType<T>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject();
                        _instance = go.AddComponent<T>();
                        go.name = $"{typeof(T)} (Singleton)";
                    
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return _instance;
        }
    }

    // 애플리케이션이 종료될 때 인스턴스 생성을 방지하기 위해 사용
    protected void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}
