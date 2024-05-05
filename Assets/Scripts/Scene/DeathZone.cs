using UnityEngine;
using UnityEngine.Events;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private string playerTag;
    [SerializeField]
    private GameSpeed _gameSpeed;
    [SerializeField]
    private Rigidbody2D _ridbody2d;


    private void Awake()
    {
        if(_ridbody2d == null)
            _ridbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _ridbody2d.velocity = _gameSpeed.CurrentVelocity;
    }
}
