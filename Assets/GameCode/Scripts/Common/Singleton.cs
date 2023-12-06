using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // 如果实例不存在，则在场景中查找
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // 如果仍然找不到，就新建一个
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 确保实例是唯一的
        if (_instance == null)
        {
            _instance = this as T;
            // 可选：使对象在加载新场景时不被销毁
            //DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}