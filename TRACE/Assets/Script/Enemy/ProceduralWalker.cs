using UnityEngine;
using System.Collections;

public class ProceduralWalker : MonoBehaviour
{
    [Header("References")]
    public Transform body;
    public Transform leftFootTarget;
    public Transform rightFootTarget;

    [Header("Step Settings")]
    public float stepDistance = 0.5f;
    public float stepHeight = 0.2f;
    public float stepSpeed = 4f;
    public float moveSpeed = 1.5f;

    private bool leftMoving = false;
    private bool rightMoving = false;

    void Update()
    {
        // Движи тялото напред
        body.position += body.forward * moveSpeed * Time.deltaTime;

        // Проверка кога да се направи стъпка
        if (!leftMoving && Vector3.Distance(leftFootTarget.position, body.position - body.right * 0.2f) > stepDistance)
        {
            StartCoroutine(StepLeftFoot(body.position - body.right * 0.2f));
        }

        if (!rightMoving && Vector3.Distance(rightFootTarget.position, body.position + body.right * 0.2f) > stepDistance)
        {
            StartCoroutine(StepRightFoot(body.position + body.right * 0.2f));
        }
    }

    IEnumerator StepLeftFoot(Vector3 newHome)
    {
        leftMoving = true;
        Vector3 startPos = leftFootTarget.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * stepSpeed;
            Vector3 pos = Vector3.Lerp(startPos, newHome, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * stepHeight;
            leftFootTarget.position = pos;
            yield return null;
        }

        leftFootTarget.position = newHome;
        leftMoving = false;
    }

    IEnumerator StepRightFoot(Vector3 newHome)
    {
        rightMoving = true;
        Vector3 startPos = rightFootTarget.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * stepSpeed;
            Vector3 pos = Vector3.Lerp(startPos, newHome, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * stepHeight;
            rightFootTarget.position = pos;
            yield return null;
        }

        rightFootTarget.position = newHome;
        rightMoving = false;
    }
}
