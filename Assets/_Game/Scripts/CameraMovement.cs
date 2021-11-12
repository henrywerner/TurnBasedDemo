using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _focalPoint;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            transform.RotateAround(_focalPoint.transform.position, Vector3.up, 20 * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.RotateAround(_focalPoint.transform.position, Vector3.up, -20 * Time.deltaTime);
    }
}
