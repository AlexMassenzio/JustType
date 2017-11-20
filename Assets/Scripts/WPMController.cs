using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WPMController : MonoBehaviour {

    float wpm = 0f;
    InputController ic = null;
    float deadAir = 0f;
    float deadAirFactor = 0.15f;
    public struct Word
    {
        public string text;
        public float time;
    }

    List<Word> completedWords = new List<Word>();

    public Word hardestWord;

	// Use this for initialization
	void Start () {
        hardestWord = new Word
        {
            text = "",
            time = 0
        };
	}
	
	// Update is called once per frame
	void Update () {
		if (ic == null)
        {
            ic = GameObject.FindGameObjectWithTag("Player").GetComponent<InputController>();
        }

        if (ic != null && GameObject.FindGameObjectsWithTag("Missile").Length > 0)
        {
            deadAir += Time.deltaTime;
        }
	}

    public void CompleteWord(string word, float time)
    {
        if (time <= 0)
        {
            time = 0.005f;
        }

        Debug.Log("Completed " + word + " in " + time.ToString() + " seconds");

        Word w = new Word {
            text = word,
            time = time
        };

        if (hardestWord.text == "" || w.time / w.text.Length > hardestWord.time / hardestWord.text.Length)
        {
            hardestWord = w;
            Debug.Log("Set hardestWord to " + hardestWord.text);
        }

        completedWords.Add(w);
        wpm = GetWPM();

        PopulateText();
    }

    void PopulateText()
    {
        GetComponent<UnityEngine.UI.Text>().text = "WPM: " + wpm.ToString();
    }

    public float GetWPM()
    {
        
        int characters = 0;
        float timeTaken = 0;
        for (var i = 0; i < completedWords.Count; i++)
        {
            characters += completedWords[i].text.Length;
            timeTaken += completedWords[i].time;
        }


        float charactersPerSecond = (float) characters / (timeTaken + (deadAir * deadAirFactor));

        //people simplify to 5 characters being one word, so wpm is equal to (cps / 5) * 60
        return (charactersPerSecond / 5) * 60;
    }
}
