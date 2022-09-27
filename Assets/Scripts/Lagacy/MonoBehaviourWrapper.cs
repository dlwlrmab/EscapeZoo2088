using UnityEngine;

public abstract class MonoBehaviourWrapper : MonoBehaviour
{
    public GameObject CachedGameObject;
    public Transform CachedTransform;

    public new GameObject gameObject
    {
        get
        {
            return null;
        }
    }

    public new Transform transform
    {
        get
        {
            return null;
        }
    }

    protected virtual void Reset()
    {
        CachedTransform = base.transform;
        CachedGameObject = base.gameObject;
    }

    protected virtual void Awake()
    {
        CachedGameObject = base.gameObject;
        CachedTransform = base.transform;
    }
}
