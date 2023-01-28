using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GPT3Script : MonoBehaviour
{
	public string prompt = "Your Prompt Here";
	public string apiKey = "YOUR API KEY";

	// The engine you want to use (keep in mind that it has to be the exact name of the engine)
	private string model = "text-davinci-003";
	public float temperature = 0.5f;
	public int maxTokens = 200;

	void Start()
	{
		StartCoroutine(MakeRequest());
	}

	IEnumerator MakeRequest()
	{
		// Create a JSON object with the necessary parameters
		var json = "{\"prompt\":\"" + prompt + "\",\"model\":\"" + model + "\",\"temperature\":" + temperature + ",\"max_tokens\":" + maxTokens + "}";
		byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

		// Create a new UnityWebRequest
		var request = new UnityWebRequest("https://api.openai.com/v1/completions", "POST");
		request.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
		request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		request.SetRequestHeader("Authorization", "Bearer " + apiKey);

		// Send the request
		yield return request.SendWebRequest();

		// Check for errors
		if (request.isNetworkError || request.isHttpError)
		{
			Debug.Log(request.error);
		}
		else
		{
			// Deserialize the JSON response
			var response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
			Debug.Log(response.choices[0].text.TrimStart().TrimEnd());
		}
	}

	// A class to hold the JSON response
	[System.Serializable]
	private class Response
	{
		public Choice[] choices;
	}

	[System.Serializable]
	private class Choice
	{
		public string text;
	}
	}
