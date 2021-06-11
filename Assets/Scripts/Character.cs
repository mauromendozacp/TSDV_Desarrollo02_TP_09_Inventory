using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    public float moveSpeed = 5;
    public float turnSpeed = 250;
    private Coroutine _rotateCorroutine = null;

    public IEnumerator Rotate()
    {
        Quaternion toRot = Quaternion.LookRotation(GetDirection(), Vector3.up);

        while (transform.rotation != toRot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, turnSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public Vector3 GetDirection()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    public void SetMovement(Vector3 dir, Vector3 newDir)
    {
        if (dir != newDir)
        {
            //StopAllCoroutines();
            if (_rotateCorroutine != null)
            {
                StopCoroutine(_rotateCorroutine);
                _rotateCorroutine = null;
            }

            _rotateCorroutine = StartCoroutine(Rotate());
        }

        transform.position += newDir * moveSpeed * Time.deltaTime;
    }
}
