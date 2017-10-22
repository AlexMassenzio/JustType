using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public string currentWord;
    public InputReceiver target;

    private int targetPriority = 0;

	// Use this for initialization
	void Start () {
        currentWord = "";
        target = null;
        GameObject.FindObjectOfType<InputField>().Select();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab))
        {
            AttemptSwitchTarget();
        }
    }

    public void AttemptSwitchTarget()
    {
        if (currentWord.Length >= 1)
        {
            List<InputReceiver> potentialTargets = new List<InputReceiver>();

            targetPriority += 1;

            foreach (InputReceiver ir in GameObject.FindObjectsOfType<InputReceiver>())
            {
                if (ir.word.StartsWith(currentWord))
                {
                    potentialTargets.Add(ir);
                }
            }

            //yay basic insertion sort
            List<InputReceiver> sortedTargets = new List<InputReceiver>();
            int desiredIndex = -1;
            int prevID = -1;
            int currentMinimum = -1;
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                currentMinimum = -1;
                for (int j = 0; j < potentialTargets.Count; j++)
                {
                    if ((prevID < 0 || potentialTargets[j].ID > prevID) && (currentMinimum < 0 || potentialTargets[j].ID < currentMinimum))
                    {
                        desiredIndex = j;
                        currentMinimum = potentialTargets[j].ID;
                    }
                }
                prevID = potentialTargets[desiredIndex].ID;
                sortedTargets.Add(potentialTargets[desiredIndex]);
            }

            if (target != null)
            {
                target.PlayerUnlock();
                targetPriority = targetPriority % sortedTargets.Count;
                target = sortedTargets[targetPriority];
                target.PlayerLock(this);
                target.ReceiveInput(currentWord);
            }
        }
    }

    public void loseTarget()
    {
        //missile destroyed base, forcibly lock off
        target = null;
        currentWord = "";
        this.GetComponent<InputField>().text = currentWord;
    }

    public void DestroyTarget()
    {
        //we successfully destroyed the whole target
        target = null;
        currentWord = "";
        this.GetComponent<InputField>().text = currentWord;

        //TODO: score?
    }

    public void ValueChanged()
    {
        string fieldWord = this.GetComponent<InputField>().text;
        if (currentWord.Length == 0 && fieldWord.Length > 0)
        {
            //we are starting a new word
            //todo: search through all InputReceivers, see if any start with this letter, if yes, "lock on" to them
            int currentID = -1;
            InputReceiver tentativeTarget = null;
            foreach (InputReceiver ir in GameObject.FindObjectsOfType<InputReceiver>())
            {
                if (ir.word[0] == fieldWord[0] && (currentID == -1 || ir.ID < currentID))
                {
                    tentativeTarget = ir;
                    currentID = tentativeTarget.ID;
                }
            }
            if (currentID < 0)
            {
                //we didn't find a word, no lock-on and don't actually type that character
                currentWord = "";
            }
            else
            {
                currentWord = fieldWord;
                target = tentativeTarget;
                target.PlayerLock(this); //this...should work?
                target.ReceiveInput(currentWord);
            }
        } else if (fieldWord.Length > 0 && fieldWord[fieldWord.Length - 1] == ' ') //disallowed characters
        {
            fieldWord = fieldWord.Substring(0, fieldWord.Length - 1);
            currentWord = fieldWord;
        } else if (fieldWord.Length == 0)
        {
            if (target != null)
                target.PlayerUnlock();
            target = null;
            currentWord = fieldWord;
        } else
        {
            currentWord = fieldWord;
            if (currentWord.Length > target.word.Length + 1)
                currentWord = currentWord.Substring(0, target.word.Length);
            target.ReceiveInput(currentWord);
        }

        this.GetComponent<InputField>().text = currentWord;
    }
}
