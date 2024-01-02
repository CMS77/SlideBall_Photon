using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AuthenticationScript : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text resultText;
    public Button Login;

    private string serverUrl = "http://localhost:8080/api/users/auth";

    private void Start()
    {
        // Attach the Authenticate method to the button click event
        Login.onClick.AddListener(Authenticate);
    }
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

                resultText.text = "Authentication successful. ";
            }
            else
            {
                resultText.text = "Authentication failed.";
            }
        }
    }
}
