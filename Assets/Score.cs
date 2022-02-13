using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public float score = 0;
    public float scoreSpeed;
    public Text scoreText;
    public Rigidbody rb;

    // Increment score based on time
    void Update()
    {
        var vel = rb.velocity;
        float speed = vel.magnitude;
        Debug.Log(speed);
        if (speed > scoreSpeed)
        {
            score += Time.deltaTime;
        }

        DisplayScore(score);
    }

    // Update score
    void DisplayScore(float scoreToDisplay)
    {
        float score = Mathf.FloorToInt(scoreToDisplay);
        scoreText.text = score.ToString();
    }
}
