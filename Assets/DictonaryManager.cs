using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictonaryManager : MonoBehaviour {

    [SerializeField]
    private string fileName;

    private List<string> wordBank;
    public enum DictonaryMode { Ordered, Random };
    public DictonaryMode dictonaryMode;

	// Use this for initialization
	void Awake () {
        dictonaryMode = DictonaryMode.Ordered;

        wordBank = new List<string>();

        TextAsset fileContents = Resources.Load(fileName) as TextAsset;

        wordBank.AddRange(fileContents.text.Split(new char[] { ',', ' ', '\n', '\t'}));

        wordBank.RemoveAll(str => String.IsNullOrEmpty(str));

        foreach (string word in wordBank)
        {
            Debug.Log(word);
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
}
