using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SubmitFormOnline : MonoBehaviour
{
	public Button cancelButton;
	public Button submitButton;

	// FormID comes from the google drive ID, eg:
	// https://docs.google.com/forms/d/_YOURFORMID_/edit
	private string formID = "1CBbGXCNm7MicvsOADZClu3t7W020MIkwAjufwGrXTWM";

	// fieldID comes from the published feedback form. You can pull the value from the HTML source of the form:
	private const string wpmFieldID = "entry.335961608";
	private const string hardestWordFieldID = "entry.428805445";
	private const string survivedID = "entry.93529389";

	private bool lastError = false;

	private const float POPUP_TRANSITION_TIME = 0.25f;

	IEnumerator SendForm(string wpm, string hWord, bool survived)
	{
		//cancelButton.interactable = false;
		//submitButton.interactable = false;
		// Prevent spamming of the button when the field is empty:
		if (string.IsNullOrEmpty(hWord)) yield break;

		WWW w;

		WWWForm form = new WWWForm();
		form.AddField(wpmFieldID, wpm); // "Your Name" if you were doing the HTML entry.
		form.AddField(hardestWordFieldID, hWord);
		form.AddField(survivedID, survived.ToString());

		string url = "https://docs.google.com/forms/d/" + formID + "/formResponse";
		w = new WWW(url, form.data);
		yield return w;
		if (string.IsNullOrEmpty(w.error))
		{
			Debug.Log("Sent feedback :)");
			lastError = false;
		}
		else
		{
			Debug.Log("Failed to send feedback :(.");
			lastError = true;
		}

		//submitButton.interactable = true;
		//cancelButton.interactable = true;
		Time.timeScale = 1;
		this.gameObject.SetActive(false);
	}

	// Button activation from UI:
	public void Activate()
	{
		string wpm = FindObjectOfType<WPMController>().GetWPM().ToString();
		string hardestWord = FindObjectOfType<WPMController>().hardestWord.text;
		bool survived = GameObject.FindGameObjectsWithTag("Base").Length > 0;

		StartCoroutine(SendForm(wpm, hardestWord, survived));
	}

	public void Cancel() //NOT BEING USED CURRENTLY. CONSIDER REMOVING IN FUTURE
	{
		Time.timeScale = 1;
		this.gameObject.SetActive(false);
	}
}