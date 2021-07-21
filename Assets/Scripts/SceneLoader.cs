using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
    }

    public void LoadLevel(int index) {
        SceneManager.LoadScene(index);
    }
}
