using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReattachHead : MonoBehaviour
{
    float timer;
    float leapTime;

    bool reattaching = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HeadMount" && !reattaching)
        {
            reattaching = true;
            Reattach(other.gameObject);
        }
    }

    public void Reattach(GameObject target)
    {
        timer = leapTime = .8f;
        StartCoroutine(LeapToTarget(transform.position, target));
    }

    IEnumerator LeapToTarget(Vector3 start, GameObject target)
    {
        Vector3 targetPos = target.GetComponent<HeadAnchor>().headLocation.transform.position;
        targetPos.y += .3f; //needs to add some to Y because Unity isn't working properly

        MovementManager.Instance.canMove = false;
        GetComponent<Rigidbody>().useGravity = false;
        while (timer > 0)
        {
            float currentTime = (leapTime - timer);
            Vector3 currentPos = Vector3.Lerp(start, targetPos, currentTime / leapTime);
            float addition = -((1.5f * currentTime - (leapTime / 2))*(1.5f * currentTime - (leapTime / 2))) + (leapTime / 2);
            currentPos.y += addition;
            GetComponent<Rigidbody>().position = currentPos;

            timer -= Time.deltaTime;

            yield return null;
        }
        reattaching = false;
        PlayerManager.Instance.SwapEffigy(1, target);
    }
}
