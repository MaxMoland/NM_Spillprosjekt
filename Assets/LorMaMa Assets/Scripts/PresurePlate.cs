using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour {

    public GameObject[] _ObjectsToActivate;

    public GameObject _DesiredObject;


    private void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < 0.8f)
        {
        Debug.Log("Trigger entered!");
        Triggered();
        }
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) > 1.2f)
        {
            DeTriggered();
        }


    }


    public void Triggered()
    {
        foreach (var item in _ObjectsToActivate)
        {
            item.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
    }
    public void DeTriggered()
    {
        foreach (var item in _ObjectsToActivate)
        {
            item.SetActive(false);
            GetComponent<AudioSource>().Stop();
        }
    }
}
