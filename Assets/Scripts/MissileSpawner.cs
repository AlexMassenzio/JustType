﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour {

    public float spawnRate = 2f;
    public GameObject missile;
    private DictionaryManager dm;
    public int numberOfMissiles = 25;

	// Use this for initialization
	void Start () {
        dm = FindObjectOfType<DictionaryManager>();

        StartCoroutine(Spawner());
	}
	
	IEnumerator Spawner()
    {
        while(!dm.IsEmpty() && numberOfMissiles > 0)
        {
            Instantiate(missile);
            spawnRate *= 0.99f;
            if (spawnRate < 0.25f)
                spawnRate = 0.25f;
            numberOfMissiles -= 1;
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
