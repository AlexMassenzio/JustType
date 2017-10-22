using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour {

    [SerializeField]
    private string fileName;

    private List<string> wordBank;
    public enum DictonaryMode { Ordered, Random };
    public DictonaryMode dictonaryMode = DictonaryMode.Ordered;

    private int currentID = 0;

	// Use this for initialization
	void Awake () {
        if (fileName != "")
        {
            wordBank = new List<string>();

            TextAsset fileContents = Resources.Load(fileName) as TextAsset;

            string[] readWords = fileContents.text.Split(new char[] { ',', ' ', '\n', '\t' });

            foreach (string s in readWords)
            {
                wordBank.Add(s.Trim());
            }

            wordBank.RemoveAll(str => String.IsNullOrEmpty(str));

            foreach (string word in wordBank)
            {
                Debug.Log(word);
            }
        }
        else
        {
            Debug.LogError("Filename is currently blank. Set one!", this);
        }
    }

    /// <summary>
    /// Returns the next word based on the dictonaryMode. Calling this function will also remove the word from the internal word bank.
    /// </summary>
    public string LoadNextWord()
    {
        int indexToGet = 0;

        if(dictonaryMode == DictonaryMode.Random)
        {
            indexToGet = UnityEngine.Random.Range(0, wordBank.Count - 1);
        }

        string nextWord = wordBank[indexToGet];
        wordBank.RemoveAt(indexToGet);

        return nextWord;
    }

    public int LoadNextID()
    {
        return currentID++;
    }

    public bool IsEmpty()
    {
        return wordBank.Count == 0;
    }
}
