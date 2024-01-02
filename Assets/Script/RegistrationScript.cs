using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RegistrationScript : MonoBehaviour
{
    public InputField emailInput;
    public InputField usernameInput;
    public InputField passwordInput;
    public Text resultText;
    public Button Register;

    private string serverUrl = "http://localhost:8080/api/users/";

    private void Start()
    {
        // Attach the Register method to the button click event
        Register.onClick.AddListener(RegisterUser);
    }

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

        using (UnityWebRequest www = UnityWebRequest.Put(serverUrl, jsonPayload))
        {
            www.method = "POST"; // Set the method to POST
            www.SetRequestHeader("Content-Type", "application/json"); // Set content type to JSON

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                resultText.text = "Registration successful.";
            }
            else
            {
                resultText.text = "Registration failed.";
            }
        }
    }
}
