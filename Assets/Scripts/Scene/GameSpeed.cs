using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSpeed : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float movSpeed, minSpeed, maxSpeed;
    

    Vector2 _currentVelocity;
    public Vector2 CurrentVelocity
    {
        get { return _currentVelocity; }
        set
        {
            if (value.x < maxSpeed)
                _currentVelocity = value;
            else
                OnReachMaxVelocity?.Invoke();
        }
    }

    public UnityEvent OnReachMaxVelocity;

    private void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(minSpeed, 0);
        CurrentVelocity = rb.velocity;
    }

    void Update()
    {
        if (rb.velocity.x < maxSpeed)
        {
            rb.AddForce(Vector2.right * movSpeed);
            CurrentVelocity = rb.velocity;
        }
    }
}
