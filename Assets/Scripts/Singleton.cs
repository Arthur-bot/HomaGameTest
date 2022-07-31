using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    #region Fields

    private static T _instance;

    #endregion

    #region Properties

    public static bool HasInstance { get; private set; }

    public static T Instance
    {
        get
        {
            if (HasInstance) return _instance;

            var name = typeof(T).Name.ToUpper();
            var asset = FindObjectOfType<T>();
            
            if (asset != null)
            {
                if (Application.isPlaying)
                {
                    asset.Awake();
                }
                else
                {
                    _instance = asset;
                    HasInstance = true;
                }

                return _instance;
            }

            var go = new GameObject($"[{name.ToUpperInvariant()}]");

            _instance = go.GetComponent<T>();

            if (_instance == null)
            {
                _instance = go.AddComponent<T>();
                HasInstance = true;
            }


            return _instance;
        }
    }

    public virtual bool UseDontDestroyOnLoad => false;

    #endregion

    #region Protected Methods

    protected virtual void OnAwake() { }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        if (!Application.isPlaying) return;

        if (_instance != null)
        {
            if (_instance != this)
            {
                DestroyImmediate(gameObject);
            }

            return;
        }

        _instance = (T)this;
        HasInstance = true;
        
        if (UseDontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        OnAwake();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            HasInstance = false;
        }
    }

    #endregion
}