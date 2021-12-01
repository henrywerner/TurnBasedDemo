using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _focalPoint;

    private void Start()
    {
        transform.LookAt(_focalPoint.transform);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * 35 * Time.deltaTime);
            //transform.RotateAround(_focalPoint.transform.position, Vector3.up, 35 * Time.deltaTime);
            transform.LookAt(_focalPoint.transform);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * 35 * Time.deltaTime);
            //transform.RotateAround(_focalPoint.transform.position, Vector3.left, 35 * Time.deltaTime);
            transform.LookAt(_focalPoint.transform);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * 35 * Time.deltaTime);
            transform.LookAt(_focalPoint.transform);
            //transform.RotateAround(_focalPoint.transform.position, Vector3.down, 35 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 35 * Time.deltaTime);
            //transform.RotateAround(_focalPoint.transform.position, Vector3.right, 35 * Time.deltaTime);
            transform.LookAt(_focalPoint.transform);
        }
        /*
        if (Input.GetKey(KeyCode.D))
            transform.RotateAround(_focalPoint.transform.position, Vector3.up, 35 * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.RotateAround(_focalPoint.transform.position, Vector3.up, -35 * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            transform.RotateAround(_focalPoint.transform.position, Vector3.right, 35 * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.RotateAround(_focalPoint.transform.position, Vector3.right, -35 * Time.deltaTime);
        */
    }
}
