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
    bool mWalking = false;
    bool mFlippedX = false;
    bool mGrounded = false;
    bool mJumping = false;

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
            mWalking = true;
            mFlippedX = false;
        }
        else if (mMoveInput < 0f)
        {
            mWalking = true;
            mFlippedX = true;
        }
        else
        {
            mWalking = false;
        }

        //jump
        if (Input.GetButtonDown("Jump") && mRigidbody.linearVelocityY < 0.01f && mRigidbody.linearVelocityY > -0.01f)
        {
            mRigidbody.AddForce(Vector2.up * mJumpForce, ForceMode2D.Impulse);

            mJumping = true;
            mGrounded = false;
        }

        //fall check
        if (mRigidbody.linearVelocityY < -0.01f)
        {
            mJumping = false;
            mGrounded = false;
        }

        //ground check
        if (!mGrounded && !mJumping && mRigidbody.linearVelocityY > -0.01f)
        {
            mGrounded = true;
        }

        Animate();

    }

    void Animate()
    {
        pSr.flipX = mFlippedX;
        pAnim.SetBool("Walking", mWalking);
        pAnim.SetBool("Grounded", mGrounded);
        pAnim.SetBool("Jumping", mJumping);

        hSr.flipX = mFlippedX;
        hAnim.SetBool("Grounded", mGrounded);
        hAnim.SetBool("Jumping", mJumping);
    }
}
