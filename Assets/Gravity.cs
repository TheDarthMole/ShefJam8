using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Gravity : MonoBehaviour
{
    public Transform gravityTarget;
    public Vector3 gravityTargetVec;
    public Transform cameraTransform;
    public MeshCollider mobiusColider;

    public MeshFilter mobiusFilter; 
    // float gravity = 9.8f;
    
    public float mobiusRadius = 15f;
    LayerMask WorldLayerMask = LayerMask.GetMask();
    
    
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
    }

     public class BaryCentricDistance {
     
     public BaryCentricDistance(MeshFilter meshfilter)
     {
         _meshfilter = meshfilter;
         _mesh = _meshfilter.sharedMesh;
         _triangles = _mesh.triangles;
         _vertices = _mesh.vertices;
         _transform = meshfilter.transform;
     }
     
     public struct Result
     {
         public float distanceSquared;
         public float distance
         {
             get
             {
                 return Mathf.Sqrt(distanceSquared);
             }
         }
         
         public int triangle;
         public Vector3 normal;
         public Vector3 centre;
         public Vector3 closestPoint;
     }
     
     int[] _triangles;
     Vector3[] _vertices;
     Mesh _mesh;
     MeshFilter _meshfilter;
     Transform _transform;
     
     public Result GetClosestTriangleAndPoint(Vector3 point)
     {
         
         point = _transform.InverseTransformPoint(point);
         var minDistance = float.PositiveInfinity;
         var finalResult = new Result();
         var length = (int)(_triangles.Length/3);
         for(var t = 0; t < length; t++)
         {
             var result = GetTriangleInfoForPoint(point, t);
             if(minDistance > result.distanceSquared)
             {
                 minDistance = result.distanceSquared;
                 finalResult = result;
             }
         }
         finalResult.centre = _transform.TransformPoint(finalResult.centre);
         finalResult.closestPoint = _transform.TransformPoint(finalResult.closestPoint);
         finalResult.normal = _transform.TransformDirection(finalResult.normal);
         finalResult.distanceSquared = (finalResult.closestPoint - point).sqrMagnitude;
         return finalResult;
     }
     
     Result GetTriangleInfoForPoint(Vector3 point, int triangle)
     {
         Result result = new Result();
         
         result.triangle = triangle;
         result.distanceSquared = float.PositiveInfinity;
         
         if(triangle >= _triangles.Length/3)
             return result;
         
         
         //Get the vertices of the triangle
         var p1 = _vertices[ _triangles[0 + triangle*3] ];
         var p2 = _vertices[ _triangles[1 + triangle*3] ];
         var p3 = _vertices[ _triangles[2 + triangle*3] ];
         
         result.normal = Vector3.Cross((p2-p1).normalized, (p3-p1).normalized);
         
         //Project our point onto the plane
         var projected = point + Vector3.Dot((p1 - point), result.normal) * result.normal;
         
         //Calculate the barycentric coordinates
         var u = ((projected.x * p2.y) - (projected.x * p3.y) - (p2.x * projected.y) + (p2.x * p3.y) + (p3.x * projected.y) - (p3.x  * p2.y)) /
                 ((p1.x * p2.y)  - (p1.x * p3.y)  - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y)  - (p3.x * p2.y));
         var v = ((p1.x * projected.y) - (p1.x * p3.y) - (projected.x * p1.y) + (projected.x * p3.y) + (p3.x * p1.y) - (p3.x * projected.y))/
                 ((p1.x * p2.y)  - (p1.x * p3.y)  - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y)  - (p3.x * p2.y));
         var w = ((p1.x * p2.y) - (p1.x * projected.y) - (p2.x * p1.y) + (p2.x * projected.y) + (projected.x * p1.y) - (projected.x * p2.y))/
                 ((p1.x * p2.y)  - (p1.x * p3.y)  - (p2.x * p1.y) + (p2.x * p3.y) + (p3.x * p1.y)  - (p3.x * p2.y));
         
         result.centre = p1 * 0.3333f + p2 * 0.3333f + p3 * 0.3333f;
         
         //Find the nearest point
          var vector = (new Vector3(u,v,w)).normalized;
            
 
         //work out where that point is
         var nearest = p1 * vector.x + p2 * vector.y + p3 * vector.z;    
         result.closestPoint = nearest;
         result.distanceSquared = (nearest - point).sqrMagnitude;
         
         if(float.IsNaN(result.distanceSquared))
         {
             result.distanceSquared = float.PositiveInfinity;
         }
         return result;
     }
     
 }
    
    
    // private void XFixedUpdate()
    // {
    //     ProcessInput();
    //     ProcessGravity();
    //     
    // }
    //
    // void ProcessInput()
    // {
    //     Debug.Log("mobius" + gravityTarget.position);
    //     //assumes the strip centre is (0,0)
    //     //otherwise,
    //     //  m = (rb.position.y - gravityTarget.position.y) / (rb.position.x - gravityTarget.position.x)
    //     //  c = (m * rb.position.x) / rb.position.y;
    //     
    //     float obX = _rb.position.x;
    //     float obZ = _rb.position.z;
    //     
    //
    //     // float m = (rb.position.y) / (rb.position.x);
    //     // float c = 1;
    //
    //     // double cx = mobiusRadius * (obX / Math.Sqrt((obX * obX) + (obZ * obZ)));
    //     // double cz = mobiusRadius * (obZ / Math.Sqrt((obX * obX) + (obZ * obZ)));
    //
    //     // divide by 2 for scalings sake (the strip is scaled by 2)
    //     double vecSize = Math.Sqrt(obX * obX + obZ * obZ) * 2;
    //     
    //     double cx = mobiusRadius * obX / (vecSize);
    //     double cz = mobiusRadius * obZ / (vecSize);
    //     
    //     // double cx = (-obX * obZ + obX / obZ) / (1 + obX * obX);
    //     // double cz = (mobiusRadius * mobiusRadius) - (cx * cx);
    //     
    //     _gravPoint = new Vector3( (float)cx, 0, (float)cz);
    //     
    //     Debug.Log(_gravPoint);
    //     
    // }
    //
    // void ProcessGravity()
    // {
    //     Vector3 diff = transform.position - _gravPoint;
    //     _rb.AddForce(- diff.normalized * gravity * (_rb.mass));
    // }
    //
    // Update is called once per frame

    
    //
    // private void UpdatePlayerTransform(Vector3 movementDirection)
    // {
    //     RaycastHit hitInfo;
    //
    //     if (GetRaycastDownAtNewPosition(movementDirection, out hitInfo))
    //     {
    //         Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
    //         Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, float.PositiveInfinity);
    //
    //         transform.rotation = finalRotation;
    //         transform.position = hitInfo.point + hitInfo.normal * .5f;
    //     }
    // }
    //
    // private bool GetRaycastDownAtNewPosition(Vector3 movementDirection, out RaycastHit hitInfo)
    // {
    //     float Speed = 1;
    //     
    //     Vector3 newPosition = transform.position;
    //     Ray ray = new Ray(newPosition + movementDirection * Speed, -transform.up);        
    //
    //     if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, WorldLayerMask))
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }
    // private void FixedUpdate()
    // {
    //     float dampening = 0.5f;
    //     float rotationDamping = 0.5f;
    //     
    //     // Calculate and set camera position
    //     Vector3 desiredPosition = gravityTarget.TransformPoint(0, 0, -1);
    //     this.transform.position = Vector3.Lerp(this.transform.position, desiredPosition, Time.deltaTime * dampening);
    //
    //     // Calculate and set camera rotation
    //     Quaternion desiredRotation = Quaternion.LookRotation(gravityTarget.position - this.transform.position, gravityTarget.up);
    //     this.transform.rotation = Quaternion.Slerp(this.transform.rotation, desiredRotation, Time.deltaTime * rotationDamping);
    // }
    //
    void Update()
    {
        Ray ray = new Ray(_rb.position, -transform.up);
        int layerMask = 1 << 8;
        RaycastHit hit;

        layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("hit!");
        }
        
        var closestPointCalculator = new BaryCentricDistance(mobiusFilter);
        var result = closestPointCalculator.GetClosestTriangleAndPoint(_rb.position);
        gravityTargetVec = result.closestPoint;
        
        
        Debug.Log(gravityTargetVec);
        
        Vector3 movementDirection = gravityTargetVec - _rb.position;
        _rb.AddForce(movementDirection);


        // UpdatePlayerTransform(new Vector3(1,0,0));

        // Vector3 trackPos = trackCollider.ClosestPointOnBounds(transform.position);
        // Vector3 wantedGravity = trackPos - transform.position;
        // _localGravity.force = wantedGravity;
        // _rb.AddForce(_localGravity.force);
    }
}
