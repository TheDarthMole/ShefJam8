using UnityEngine;
public class CarControls : MonoBehaviour
{ // Wheel Meshes
// Front
public Transform frontLeftWheelMesh;
public Transform frontRightWheelMesh;
// Rear
public Transform rearLeftWheelMesh;
public Transform rearRightWheelMesh;
// Wheel Colliders
// Front
public WheelCollider wheelFL;
public WheelCollider wheelFR;
// Rear
public WheelCollider wheelRL;
public WheelCollider wheelRR;
public float maxTorque = 500f;
public float brakeTorque = 1000f;
// wheel sub-steps
public float speedThreshold;
public int stepsBelow;
public int stepsAbove;
// max wheel turn angle;
public float maxWheelTurnAngle = 20f; // degrees
// car's center of mass
public Vector3 centerOfMass = new Vector3(0f, 0f, 0f); // unchanged
public Vector3 eulertest;
// PRIVATE
// acceleration increment counter
private float torquePower = 0f;
// turn increment counter
private float steerAngle = 30f;
void Start()
{
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        wheelFL.ConfigureVehicleSubsteps(speedThreshold, stepsBelow, stepsAbove);
        wheelFR.ConfigureVehicleSubsteps(speedThreshold, stepsBelow, stepsAbove);
        wheelRL.ConfigureVehicleSubsteps(speedThreshold, stepsBelow, stepsAbove);
        wheelRR.ConfigureVehicleSubsteps(speedThreshold, stepsBelow, stepsAbove);
    }
// Visual updates
void Update()
{
        //changing tyre direction
        Vector3 temp = frontLeftWheelMesh.localEulerAngles;
        Vector3 temp1 = frontRightWheelMesh.localEulerAngles;
        temp.y = wheelFL.steerAngle - (frontLeftWheelMesh.localEulerAngles.z);
        frontLeftWheelMesh.localEulerAngles = temp;
        temp1.y = wheelFR.steerAngle - (frontRightWheelMesh.localEulerAngles.z);
        frontRightWheelMesh.localEulerAngles = temp1;
        // Wheel rotation
        frontLeftWheelMesh.Rotate(wheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        frontRightWheelMesh.Rotate(wheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        rearLeftWheelMesh.Rotate(wheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        rearRightWheelMesh.Rotate(wheelRR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
}
// Physics updates
void FixedUpdate()
{
        // CONTROLS - FORWARD & RearWARD
        if (Input.GetKey(KeyCode.Space))
        {
            // BRAKE
            torquePower = 0f;
            wheelRL.brakeTorque = brakeTorque;
            wheelRR.brakeTorque = brakeTorque;
        }
        else
        {
            // SPEED
            torquePower = maxTorque * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
        }
        // Apply torque
        wheelRR.motorTorque = torquePower;
        wheelRL.motorTorque = torquePower;
        // CONTROLS - LEFT & RIGHT
        // apply steering to front wheels
        steerAngle = maxWheelTurnAngle * Input.GetAxis("Horizontal");
        wheelFL.steerAngle = steerAngle;
        wheelFR.steerAngle = steerAngle;
}
}