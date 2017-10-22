using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    private string assignedWord;

    [SerializeField]
    private float travelTime = 10f;

    private GameObject targetedBase;

	// Use this for initialization
	void Start () {

        assignedWord = FindObjectOfType<DictonaryManager>().LoadNextWord();

        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");

        if (bases.Length != 0)
        {
            targetedBase = bases[Random.Range(0, bases.Length - 1)];

            var sequence = LeanTween.sequence();
            sequence.append(LeanTween.move(gameObject, targetedBase.transform.position, travelTime));
            sequence.append(() =>
            {
                Destroy(targetedBase);
                Destroy(gameObject);
            });
        }
        else
        {
            Debug.LogError("No bases found! Deleting self", this);
            Destroy(this.gameObject);
        }
    }
}
