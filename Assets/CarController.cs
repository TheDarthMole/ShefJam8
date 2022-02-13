using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardAccel = 0.1f;
    public float maxSpeed = 1f;
    public float turnStrength = 180;
    public float gravityStrength = 10f;

    private float speedInput;
    private float turnInput;
    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    void Start()
    {
        System.Console.Out.Write("test");
        System.Console.Out.Write(Input.GetAxis("Vertical"));
        rb.transform.parent = null;
    }

    void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }

        turnInput = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        transform.position = rb.transform.position;
    }

    private void FixedUpdate()
    {

        if (Mathf.Abs(speedInput) > 0)
        {
            rb.AddForce(transform.forward * forwardAccel * 100f);
        }
    }
}
