using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputReceiver : MonoBehaviour {

    public string word;
    public InputController lockedOn;
    public Text baseText;
    public Text progressText;
    public int ID;


	// Use this for initialization
	void Start () {
        lockedOn = null;
        setWord(word);
	}

    public void setWord(string wordToSet)
    {
        word = wordToSet;
        baseText.text = word;
        progressText.text = "";

        GameObject.FindObjectOfType<InputField>().Select();
    }
	
	// Update is called once per frame
	void Update () {
		
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

        //for now, just destroy
        lockedOn.DestroyTarget();

        //TODO: send message to owning missile or something

        GameObject.Destroy(this.gameObject);
    }

    public void PlayerLock(InputController i)
    {
        //player has locked onto us
        lockedOn = i;
    }

    public void PlayerUnlock()
    {
        //for some reason, player has backspaced past beginning of word, unlocked us
        lockedOn = null;
        ReceiveInput("");
    }

    public bool ReceiveInput(string progress)
    {
        //todo
        progressText.text = progress;

        bool soFarSoGood = progress.Length > 0 && progress.Length <= word.Length && progress[progress.Length - 1] == word[progress.Length - 1];
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

}
