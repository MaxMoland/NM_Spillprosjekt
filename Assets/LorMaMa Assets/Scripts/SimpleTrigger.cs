using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrigger : MonoBehaviour {

    public GameObject[] _ObjectsToActivate;
    public GameObject[] _ObjectsToDisable;

    public GameObject _DesiredObject;


    private void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < 0.8f)
        {
        Debug.Log("Trigger entered!");
        _DesiredObject.transform.position = transform.position;
        _DesiredObject.GetComponent<Pushable>().enabled = true;
        Triggered();
        }
    }


    public void Triggered()
    {
        foreach (var item in _ObjectsToActivate)
        {
            item.SetActive(true);         
        }
        foreach (var item in _ObjectsToDisable)
        {
            item.SetActive(false);
        }

    }
}
