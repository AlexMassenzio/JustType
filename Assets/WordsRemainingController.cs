using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsRemainingController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MissileSpawner m = GameObject.FindObjectOfType<MissileSpawner>();
        this.GetComponent<Text>().text = "Words remaining: " + m.numberOfMissiles;
	}
}
