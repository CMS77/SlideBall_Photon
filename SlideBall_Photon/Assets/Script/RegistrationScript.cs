using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System.Text;
using System;

public class RegistrationScript : MonoBehaviour
{
    public InputField emailInput;
    public InputField usernameInput;
    public InputField passwordInput;
    public Text resultText;
    public Button Register;

    private string serverUrl = "http://localhost:8081/api/users/";
    private string authUrl = "http://localhost:8081/api/users/auth";


    public void RegisterUser()
    {
        string email = emailInput.text;
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            resultText.text = "Username and password are required!";
            return;
        }

        StartCoroutine(RegisterUserRequest(email, username, password));
        Debug.Log("Register method called");
    }

    private IEnumerator RegisterUserRequest(string email, string username, string password)
    {
        
        
        string jsonPayload = $"{{\"email\":\"{email}\", \"name\":\"{username}\",\"pass\":\"{password}\"}}";

        var success = false;

        using (UnityWebRequest www = UnityWebRequest.Post(serverUrl, jsonPayload, "application/json"))
        {
            www.url = serverUrl;
            //www.method = "POST"; // Set the method to POST
            //www.SetRequestHeader("Content-Type", "application/json"); // Set content type to JSON

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var text = www.downloadHandler.text;
                RegistrateResult result = JsonUtility.FromJson<RegistrateResult>(text);

                resultText.text = "Registration successful.";
                success = true;
            }
            else
            {
                resultText.text = "Registration failed.";
            }
        }

        if (success)
        {
            jsonPayload = $"{{\"name\":\"{username}\",\"pass\":\"{password}\"}}";
            using (UnityWebRequest www = UnityWebRequest.Post(authUrl, jsonPayload, "application/json"))
            {
                www.url = authUrl;
                //www.method = "POST"; // Set the method to POST
                //www.SetRequestHeader("Content-Type", "application/json"); // Set content type to JSON

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    var text = www.downloadHandler.text;
                    AuthenticateResult result = JsonUtility.FromJson<AuthenticateResult>(text);
                    GameManager.setPlayerId(result.result.id.ToString());
                    Leaderboard.SetPlayerName(result.result.name);

                    resultText.text = "Auto login successful.";
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    resultText.text = "Auto login after registration failed.";
                }
            }
        }
    }

    [System.Serializable]
    class RegistrateResult
    {
        public string msg;
        public RegistratePlayer result;
    }

    [System.Serializable]
    class RegistratePlayer
    {
        public int pr_id;
        public string pr_name;
        public string pr_email;
        public string pr_pass;
        public string pr_token;
    }
}
