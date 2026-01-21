using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 mOffset;
    [SerializeField] private float mDamping;

    public Transform mPlayer;
    private Vector3 vel = Vector3.zero;

    private void Start()
    {
       
    }

    private void Update()
    {
        Vector3 targetPos = mPlayer.position + mOffset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, mDamping);
    }
}
