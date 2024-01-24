using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class AuthenticationScript : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text resultText;
    public Button Login;

    private string serverUrl = "http://localhost:8081/api/users/auth";

    public void Authenticate()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            resultText.text = "Username and password are required!";
            return;
        }
        StartCoroutine(AuthenticateRequest(username, password));
        Debug.Log("Authenticate method called");
    }

    private IEnumerator AuthenticateRequest(string username, string password)
    {
        string jsonPayload = $"{{\"name\":\"{username}\",\"pass\":\"{password}\"}}";

        using (UnityWebRequest www = UnityWebRequest.Put(serverUrl, jsonPayload))
        {
            www.method = "POST"; // Set the method to POST
            www.SetRequestHeader("Content-Type", "application/json"); // Set content type to JSON

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var text = www.downloadHandler.text;
                AuthenticateResult result = JsonUtility.FromJson<AuthenticateResult>(text);

                resultText.text = "Authentication successful. ";
                GameManager.setPlayerId(result.result.id.ToString());
                Leaderboard.SetPlayerName(result.result.name);
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                resultText.text = "Authentication failed.";
            }
        }
    }
    
}

[System.Serializable]
public class AuthenticateResult
{
    public string msg;
    public AutenticatePlayer result;
}

[System.Serializable]
public class AutenticatePlayer
{
    public int id;
    public string name;
    public string email;
    public string token;
}
