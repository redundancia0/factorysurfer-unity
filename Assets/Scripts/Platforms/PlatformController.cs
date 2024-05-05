using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private float xOffset = 0f;
    [SerializeField]
    private float yMinPos, yMaxPos;
    [SerializeField]
    private float xMinSize = 1f, xMaxSize = 1f;

    private ObjectPool<PlatformController> _pool;
    public ObjectPool<PlatformController> Pool 
    { 
        get { return _pool; }
        set { _pool = value; }
    }


    private void OnEnable()
    {
        transform.position = new Vector3(GameManager.Instance.MaxScreenBounds.x + xOffset, Random.Range(yMinPos, yMaxPos), transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, Random.Range(xMinSize, xMaxSize), transform.localScale.z);
    }

    private void Update()
    {
        if (GameManager.Instance.MinScreenBounds.x  > transform.position.x + xOffset)
            Release();
    }

    private void Release()
    {
        _pool.Release(this);
    }
}
