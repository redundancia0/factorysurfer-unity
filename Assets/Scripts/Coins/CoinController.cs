using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CoinController : MonoBehaviour
{
    [SerializeField]
    private float xOffset = 0f, yOffset = 0f;
    [SerializeField]
    private float _fallForce;

    [Header("References")]
    [SerializeField]
    Rigidbody2D _rb2d;
    [SerializeField]
    ParticleSystemController _particleSystem;
    [SerializeField]
    SpriteRenderer _sprite;
    [SerializeField]
    TrailRenderer _trailRenderer;

    private ObjectPool<CoinController> _pool;
    public ObjectPool<CoinController> Pool 
    { 
        get { return _pool; }
        set { _pool = value; }
    }

    private void Awake()
    {
        if(_rb2d == null)
            _rb2d = GetComponent<Rigidbody2D>();
        if(_particleSystem == null)
            _particleSystem = GetComponentInChildren<ParticleSystemController>();
        if(_sprite == null)
            _sprite = GetComponentInChildren<SpriteRenderer>();
        if(_trailRenderer == null)
            _trailRenderer = GetComponent<TrailRenderer>();

        _particleSystem.OnStop += Release;
    }

    private void OnEnable()
    {        
        transform.position = new Vector3((GameManager.Instance.MaxScreenBounds.x) + xOffset, GameManager.Instance.MaxScreenBounds.y + yOffset, transform.position.z);
        _trailRenderer.enabled = true;
        _rb2d.AddForce(Vector2.down * _fallForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Player")
            PlayerGetsCoin();
        if (tag == "DeadZone")
            Release();
    }


    private void PlayerGetsCoin()
    {
        _sprite.gameObject.SetActive(false);
        _rb2d.isKinematic = true;
        _rb2d.velocity = Vector2.zero;
        _particleSystem.PlayAnimation();
        HUDActions.AddCoin?.Invoke();
    }

    private void Release()
    {
        _trailRenderer.enabled = false;
        _rb2d.isKinematic = false;
        _sprite.gameObject.SetActive(true);
        _pool.Release(this);
    }
}
