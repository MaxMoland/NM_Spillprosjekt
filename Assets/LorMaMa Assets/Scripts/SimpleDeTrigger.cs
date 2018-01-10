using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDeTrigger : MonoBehaviour {

    public GameObject[] _ObjectsToActivate;
    public GameObject[] _ObjectsToDisable;

    public GameObject _DesiredObject;


    private void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) > 1.2f)
        {
        Debug.Log("Trigger entered!");
        Debug.Log(Vector3.Distance(transform.position, _DesiredObject.transform.position));
        //_DesiredObject.transform.position = transform.position;
        //_DesiredObject.GetComponent<Pushable>().enabled = true;
        Triggered();
        }
    }


    public void Triggered()
    {
        foreach (var item in _ObjectsToActivate)
        {
            item.SetActive(false);
            //_DesiredObject.GetComponent<Pushable>()._mass = 1;
        }
        foreach (var item in _ObjectsToDisable)
        {
            item.SetActive(true);
        }      

    }
}
