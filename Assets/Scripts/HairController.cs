using System.Collections;
using UnityEngine;

public class HairController : MonoBehaviour
{
    public Transform mParent;
    public GameObject mHairGrapple;
    public GameObject mHairSprite;

    Camera cam;
    bool mShooting;
    bool mGrappling;

    private void Start()
    {
        cam = Camera.main;
        mShooting = false;
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

        Vector2 hairCenter = mHairGrapple.transform.position;
        Vector2 hairSize = mHairGrapple.GetComponent<SpriteRenderer>().bounds.size;

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

            //hairCenter = mHairGrapple.transform.position;
            //Collider2D hit = Physics2D.OverlapBox(hairCenter, hairSize, 0f);
            //if (hit != null && hit.gameObject != mHairGrapple && hit.CompareTag("Grapple"))
            //{

            //}
            
            yield return null;
        }
        mHairGrapple.transform.localPosition = finalPos;

        StartCoroutine(RetractHair(time));
    }

    IEnumerator RetractHair(float time)
    {
        float elapsedTime = 0f;

        Vector3 startPos = Vector3.right * 10;
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
}
