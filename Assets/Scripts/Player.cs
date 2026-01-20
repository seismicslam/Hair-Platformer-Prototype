using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float mWalkSpeed = 5.0f;
    public float mJumpForce = 5.0f;
    float mMoveInput;

    Rigidbody2D mRigidbody;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mMoveInput = Input.GetAxis("Horizontal");
        mRigidbody.linearVelocity = new Vector2(mMoveInput * mWalkSpeed, mRigidbody.linearVelocityY);
        if (Input.GetButtonDown("Jump") && mRigidbody.linearVelocityY < 0.01f && mRigidbody.linearVelocityY >= 0f)
        {
            mRigidbody.AddForce(Vector2.up * mJumpForce, ForceMode2D.Impulse);
        }
    }
}
