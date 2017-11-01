using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    private string assignedWord;

    [SerializeField]
    private float travelTime = 10f;

    private Transform trans;
    private GameObject targetedBase;

	// Use this for initialization
	void Start () {
        trans = GetComponent<Transform>();

        assignedWord = FindObjectOfType<DictionaryManager>().LoadNextWord();
        GetComponentInChildren<InputReceiver>().SetWord(assignedWord);

        GetComponentInChildren<InputReceiver>().ID = FindObjectOfType<DictionaryManager>().LoadNextID();

        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");

        if (bases.Length != 0)
        {
            targetedBase = bases[Random.Range(0, bases.Length - 1)];
            StartCoroutine(SendMissile());
        }
        else
        {
            Debug.LogError("No bases found! Deleting self", this);
            Destroy(this.gameObject);
        }
    }

    IEnumerator SendMissile()
    {
        LeanTween.move(gameObject, targetedBase.transform.position, travelTime);

        yield return new WaitForSeconds(travelTime);

        this.GetComponentInChildren<InputReceiver>().GameDestroy();
        Destroy(targetedBase);
        Destroy(gameObject);
    }
}
