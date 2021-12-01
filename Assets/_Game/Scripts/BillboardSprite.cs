using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private GameObject child;
    
    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        //child.transform.rotation = Quaternion.LookRotation(_cam.transform.position) * Quaternion.Euler(0, 180, 0);
        //child.transform.localEulerAngles = new Vector3(0, child.transform.localEulerAngles.y, 0);
        /*
        child.transform.rotation = Quaternion.Euler(
            child.transform.rotation.eulerAngles.x, 
            Quaternion.LookRotation(_cam.transform.position).eulerAngles.y, 
            child.transform.rotation.eulerAngles.z);
        */
        
        child.transform.forward = _cam.transform.forward;
    }
}
