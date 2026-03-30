using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    Animator myAnimator;
    float gravityScaleAtStart;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isTouchingGround()) { return; }

        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        myAnimator.speed = 1f;

        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;

        myAnimator.SetBool("isRunning", hasHorizontalSpeed());
    }

    void FlipSprite()
    {
        if (hasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!isTouchingLadder())
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);

            return;
        }

        myRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbVelocity;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);

        if (!hasVerticalSpeed && !isTouchingGround())
        {
            myAnimator.SetBool("isClimbing", true);
            myAnimator.speed = 0f;
        } 
    }

    private bool hasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
    }

    private bool isTouchingGround()
    {
        return myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private bool isTouchingLadder()
    {
        return myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
    }
}
