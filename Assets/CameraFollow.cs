using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public Transform offsetT;
    
    private float smoothing = 5f;

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.rotation = player.transform.rotation;
        Vector3 pos = player.transform.position + (player.transform.up*0.8f)-(player.transform.forward*2f);
        transform.position = Vector3.Lerp(transform.position, pos, smoothing * Time.deltaTime);
    }
}