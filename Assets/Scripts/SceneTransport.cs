using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransport : MonoBehaviour
{
    public string loadScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(loadScene);
        }
    }
}
