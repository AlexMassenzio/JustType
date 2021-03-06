﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputReceiver : MonoBehaviour {

    public string word;
    public InputController lockedOn;
    public Text baseText;
    public Text progressText;
    public int ID;

    float timeTyping = 0f; 


	// Use this for initialization
	void Start () {
        lockedOn = null;
        //SetWord(word);
	}

    public void SetWord(string wordToSet)
    {
        word = wordToSet;
        baseText.text = word;
        progressText.text = "";

        GameObject.FindObjectOfType<InputField>().Select();
    }
	
	// Update is called once per frame
	void Update () {
		if (lockedOn != null)
        {
            timeTyping += Time.deltaTime;
        }
	}

    public void GameDestroy()
    {
        //ie the missle reaches its end before word gets typed
        if (lockedOn != null)
        {
            lockedOn.loseTarget();
            Destroy(gameObject);
        }
    }

    public void PlayerDestroy()
    {
        //player successfully types whole word
        //send message to owning missile (or anything else)


        GameObject g = GameObject.FindGameObjectWithTag("WPM");
        WPMController w = g.GetComponent<WPMController>();
        Debug.Log(g);
        Debug.Log(w);
        w.CompleteWord(word, timeTyping);

        //for now, just destroy
        lockedOn.DestroyTarget();

        //TODO: send message to owning missile or something
        GameObject.Destroy(transform.parent.gameObject);
        GameObject.Destroy(this.gameObject);


    }

    public void PlayerLock(InputController i)
    {
        //player has locked onto us
        lockedOn = i;
        timeTyping = 0f;
    }

    public void PlayerUnlock()
    {
        //for some reason, player has backspaced past beginning of word, unlocked us
        lockedOn = null;
        timeTyping = 0f;
        ReceiveInput("");
    }

    public bool ReceiveInput(string progress)
    {
        //todo
        progressText.text = progress;

        bool soFarSoGood = progress.Length > 0 && progress.Length <= word.Length && word.StartsWith(progress);
        if (soFarSoGood)
        {
            progressText.color = Color.green;
        } else
        {
            progressText.color = Color.red;
        }

        if (progress == word)
        {
            PlayerDestroy(); //they did it
        }

        return soFarSoGood;
    }

    public void Backspace()
    {
        progressText.text = progressText.text.Substring(0, progressText.text.Length - 1);
        progressText.color = Color.red;
    }

}
