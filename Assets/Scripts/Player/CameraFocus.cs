using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float angle;
    [SerializeField] GameObject focus;

    void Update()
    {
        float height = (Mathf.Tan(angle * Mathf.PI / 180)) * distance;
        transform.position = focus.transform.position + (focus.transform.up * height) - (distance * Vector3.forward);
        transform.LookAt(focus.transform);
    }
}