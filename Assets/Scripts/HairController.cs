using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class HairController : MonoBehaviour
{
    public Transform mParent;
    public GameObject mHairGrapple;
    public GameObject mHairSprite;

    public bool mGrappling;

    Camera cam;
    bool mShooting;

    Collider2D mHairCollider;
    DistanceJoint2D mJoint;

    private void Start()
    {
        cam = Camera.main;
        mShooting = false;
        mHairCollider = mHairGrapple.GetComponent<Collider2D>();
        mJoint = transform.parent.GetComponent<DistanceJoint2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mShooting)
        {
            StartCoroutine(ShootHair(0.25f));
        }
    }

    IEnumerator ShootHair(float time)
    {
        mHairSprite.SetActive(false);
        mShooting = true;

        Vector2 hairCenter = mHairCollider.bounds.center;
        Vector2 hairSize = mHairCollider.bounds.size;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 startPos = Vector3.zero;
        Vector3 finalPos = Vector3.right * 10;

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            mHairGrapple.transform.localPosition = Vector3.Lerp(startPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            hairCenter = mHairCollider.bounds.center;
            List<Collider2D> hits = new();
            mHairCollider.Overlap(hits);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Grapple") && hit.gameObject != mHairGrapple)
                {
                    StartCoroutine(Grapple(hit.gameObject, time));
                    yield break;
                }
            }

            yield return null;
        }

        StartCoroutine(RetractHair(time));
    }

    IEnumerator RetractHair(float time)
    {
        float elapsedTime = 0f;

        Vector3 startPos = mHairGrapple.transform.localPosition;
        Vector3 finalPos = Vector3.zero;

        while (elapsedTime < time)
        {
            mHairGrapple.transform.localPosition = Vector3.Lerp(startPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mHairGrapple.transform.localPosition = finalPos;

        mHairSprite.SetActive(true);
        mShooting = false;
    }

    IEnumerator Grapple(GameObject obj, float time)
    {
        mGrappling = true;
        mJoint.enabled = true;
        mJoint.connectedAnchor = obj.transform.position;

        while (Input.GetMouseButton(0))
        {
            Vector2 direction = obj.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            mHairGrapple.transform.localPosition = Vector3.right * distance;

            if (Input.GetButton("Jump"))
            {
                mJoint.distance = Mathf.Max(1f, mJoint.distance - 0.1f);
            } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                mJoint.distance = Mathf.Min(mJoint.distance + 0.1f, 9.5f);
            }

                yield return null;
        }

        mJoint.enabled = false;
        mGrappling = false;

        StartCoroutine(RetractHair(time));
    }
}
