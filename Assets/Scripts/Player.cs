using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float mWalkSpeed = 5.0f;
    public float mJumpForce = 5.0f;
    float mMoveInput;

    Rigidbody2D mRigidbody;

    //player animator and sprite renderer
    Animator pAnim;
    SpriteRenderer pSr;

    //hair animator and sprite renderer
    [SerializeField] Animator hAnim;
    [SerializeField] SpriteRenderer hSr;

    //states (currently just used for animation but could be used for functionality)
    bool walking = false;
    bool flippedX = false;
    bool grounded = false;
    bool jumping = false;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        pAnim = GetComponent<Animator>();
        pSr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //horizontal movement
        mMoveInput = Input.GetAxis("Horizontal");
        mRigidbody.linearVelocity = new Vector2(mMoveInput * mWalkSpeed, mRigidbody.linearVelocityY);

        //walk animation
        if (mMoveInput > 0f)
        {
            walking = true;
            flippedX = false;
        }
        else if (mMoveInput < 0f)
        {
            walking = true;
            flippedX = true;
        }
        else
        {
            walking = false;
        }

        //jump
        if (Input.GetButtonDown("Jump") && mRigidbody.linearVelocityY < 0.01f && mRigidbody.linearVelocityY > -0.01f)
        {
            mRigidbody.AddForce(Vector2.up * mJumpForce, ForceMode2D.Impulse);

            jumping = true;
            grounded = false;
        }

        //fall check
        if (mRigidbody.linearVelocityY < -0.01f)
        {
            jumping = false;
            grounded = false;
        }

        //ground check
        if (!grounded && !jumping && mRigidbody.linearVelocityY > -0.01f)
        {
            grounded = true;
        }

        Animate();

    }

    void Animate()
    {
        pSr.flipX = flippedX;
        pAnim.SetBool("Walking", walking);
        pAnim.SetBool("Grounded", grounded);
        pAnim.SetBool("Jumping", jumping);

        hSr.flipX = flippedX;
        hAnim.SetBool("Grounded", grounded);
        hAnim.SetBool("Jumping", jumping);
    }
}
