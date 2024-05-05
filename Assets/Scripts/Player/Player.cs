using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameSpeed _gameSpeed;
    [SerializeField]
    private Rigidbody2D _rigidbody2d;

    [SerializeField]
    private KeyCode jumpKey, bendKey;
    [SerializeField]
    private float _jumpForce;



    [SerializeField]
    private string jumpAnimName, crouchAnimName;



    [Header("Animations")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private RuntimeAnimatorController birdAnimController;
    [SerializeField]
    private RuntimeAnimatorController armorAnimController;
    [SerializeField]
    private RuntimeAnimatorController tankAnimController;

    [SerializeField] [Min(1)]
    private int numOfJumps;

    private CapsuleCollider2D capCollider;
    private Vector3 initPos;

    private bool horizontalMove = true;
    private int currentNumOfJumps = 0;


    private float _currentDistance = 0;

    public UnityEvent OnReachMaxVelocity;


    private void Awake()
    {
        if(_rigidbody2d == null)
            _rigidbody2d = GetComponent<Rigidbody2D>();
        
        if(capCollider == null)
            capCollider = GetComponent<CapsuleCollider2D>();
        
        
    }

    private void Start()
    {
        SetAnimatorController();
        initPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(jumpKey) && currentNumOfJumps < numOfJumps)
        {
            Jump();
        }
        else if (Input.GetKeyDown(bendKey))
        {
            BendPlayer();
        }
        else if (Input.GetKeyUp(bendKey))
        {
            StandUpPlayer();
        }

        if(horizontalMove)
            MovePlayer();
    }

    // Métodos
    private void Jump()
    {
        currentNumOfJumps++;
        _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, 0);
        _rigidbody2d.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger(jumpAnimName);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.collider.tag;

        if (tag == "Ground")
            currentNumOfJumps = 0;
        else if (tag == "DeadZone" || tag == "Enemy")
            GameManager.Instance.LoadScene(SCENE.POST_GAME);
    }


    private void MovePlayer()
    {
        _rigidbody2d.velocity = new Vector2(_gameSpeed.CurrentVelocity.x, _rigidbody2d.velocity.y);
        _currentDistance = Vector3.Distance(initPos, transform.position);
        HUDActions.UpdateDistance?.Invoke(_currentDistance);
    }

    private void BendPlayer()
    {
        animator.SetBool(crouchAnimName, true);
        capCollider.size = new Vector2(0.4f, 0);
    }

    private void StandUpPlayer()
    {
        animator.SetBool(crouchAnimName, false);
        capCollider.size = new Vector2(0.4f, 0.6f);
    }

    public void OnDie()
    {
        GameManager.Instance.LoadScene(SCENE.POST_GAME);
    }

    private void SetAnimatorController()
    {
        SKIN currentSkin = GameManager.Instance.GetSkin();
        if (_currentDistance.Equals(SKIN.BIRD)) return;
        switch (currentSkin)
        {
            case SKIN.BIRD:
                animator.runtimeAnimatorController = birdAnimController;
                break;
            case SKIN.ARMOR:
                animator.runtimeAnimatorController = armorAnimController;
                break;
            case SKIN.TANK:
                animator.runtimeAnimatorController = tankAnimController;
                break;
        }
    }
}
