using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetSceneName; // The name of the scene you want to transition to

    public void OnButtonClick()
    {
        // Load the target scene when the button is clicked
        SceneManager.LoadScene(targetSceneName);
    }
}
