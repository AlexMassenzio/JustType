using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour {

    public GameObject EndGUI;

    public float spawnRate = 2f;
    public GameObject missile;
    private DictionaryManager dm;
    public int numberOfMissiles = 25;

	private bool submittedForm;

	// Use this for initialization
	void Start () {
        dm = FindObjectOfType<DictionaryManager>();

        StartSpawner();
	}

	void Update()
	{
		if((numberOfMissiles <= 0 && GameObject.FindGameObjectsWithTag("Missile").Length <= 0) || GameObject.FindGameObjectsWithTag("Base").Length <= 0)
		{
			if(!submittedForm)
			{
				FindObjectOfType<SubmitFormOnline>().Activate();
				submittedForm = true;
                EndGUI.SetActive(true);
			}
		}
	}

    public void StartSpawner()
    {
        StartCoroutine(Spawner());
        submittedForm = false;
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
