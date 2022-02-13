using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGravity : MonoBehaviour
{
    private Rigidbody _rb;
    Vector3 upAxis;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        upAxis = -Physics.gravity.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        
        int layerMask = 1 << 8;
        RaycastHit hit;
        layerMask = ~layerMask;
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log(ray);
            
            _rb.AddForce(-transform.up*500);
            // _rb.AddForce(new Vector3(0f,-500f,0f));
        }
    }
}
