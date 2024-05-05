using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    float _speed = 5f;
    
    [SerializeField]
    private float xOffset = 0f;
    [SerializeField]
    private float yMinPos, yMaxPos;

    [Header("References")]
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    TrailRenderer _trailRenderer;

    private ObjectPool<ProjectileController> _pool;
    public ObjectPool<ProjectileController> Pool 
    { 
        get { return _pool; }
        set { _pool = value; }
    }

    private void Awake()
    {
        if(rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();
        if (_trailRenderer == null)
            _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        transform.position = new Vector3(GameManager.Instance.MaxScreenBounds.x + xOffset, Random.Range(yMinPos, yMaxPos), transform.position.z);
        rb2d.velocity = Vector2.left * _speed;
        _trailRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _pool.Release(this);
        _trailRenderer.enabled = false;
    }
}
