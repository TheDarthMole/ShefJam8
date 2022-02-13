using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider _)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
