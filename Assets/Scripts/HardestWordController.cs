using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HardestWordController : MonoBehaviour {


    struct Word
    {
        public string text;
        public float time;
    }

    Word hardestWord;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        WPMController w = GameObject.FindGameObjectWithTag("WPM").GetComponent<WPMController>();
        hardestWord = new Word
        {
            text = w.hardestWord.text,
            time = w.hardestWord.time
        };

        Text t = GetComponent<Text>();
        if (hardestWord.text != "")
        {
            string s = "Hardest word: \"";
            s += hardestWord.text;
            s += "\"\n";
            s += (hardestWord.time / hardestWord.text.Length).ToString() + " seconds per character";
            t.text = s;
            Debug.Log("Set text");
        }
	}
}
