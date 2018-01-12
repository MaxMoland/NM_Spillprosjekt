using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : SimpleTrigger {

    public GameObject[] _ObjectsToActivate;
    public GameObject[] _ObjectsToDisable;

    public GameObject[] _AnimationsToTurnOn;
    public GameObject[] _AnimationsToTurnOff;

    public GameObject _DesiredObject;
    private bool active = false;
    public Color colorActive;
    public Color colorDeActive;
    private Material[] mat; 

    private void Start()
    {
        mat = GetComponent<Renderer>().materials;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < 0.8f)
        {
            Triggered();
        }
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) > 1.2f)
        {
            DeTriggered();
        }


    }


    public void Triggered()
    {
        if (!active){
            GetComponent<AudioSource>().Play();
            active = true;
            GetComponent<Renderer>().material.color = colorActive;
            
        }
    }
    public void DeTriggered()
    {
        if (active)
        {
            GetComponent<AudioSource>().Stop();
            active = false;
            GetComponent<Renderer>().material.color = colorDeActive;
        }
    }
}
