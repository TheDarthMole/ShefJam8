using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject ObstaclePrefab;

    public Vector3 center;
    public Vector3 size;

    private float startTime;
    private float time;

    private void Start()
    {
        startTime = Time.time;  
    }

    void Update()
    {
        time = Time.time - startTime;
        if (time > 10)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber > 1f - (time / 2000f))
                SpawnObstacle();
        }
        
    }

    public void SpawnObstacle()
    {
        // inner and outer room
        float innerRadius = 13;
        float outerRadius = 17;

        // calculated
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(Random.Range(ratio * ratio, 1f)) * outerRadius;
        Vector3 pos = Random.insideUnitCircle.normalized * radius;

        pos = Quaternion.Euler(90, 0, 0) * pos;
        pos.z -= 15;
        float randomNumber = Random.Range(-1.5f, 1.5f);
        pos.y += randomNumber;

        Instantiate(ObstaclePrefab, pos, Random.rotation);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
