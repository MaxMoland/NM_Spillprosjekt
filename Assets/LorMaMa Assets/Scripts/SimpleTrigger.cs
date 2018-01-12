using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrigger : MonoBehaviour {

    public GameObject[] _ObjectsToActivate;
    public GameObject[] _ObjectsToDisable;

    private PlayerBehaviour _playerBehaviour;

    //public GameObject[] _AnimationsToTurnOn;
    //public GameObject[] _AnimationsToTurnOff;

    public GameObject _DesiredObject;

    public bool _triggered = false;

    private void Start()
    {
        _playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }


    private void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < 0.8f && _playerBehaviour.GetState() != PlayerBehaviour.State.Pushing)
        {
            Debug.Log("Trigger entered!");
            _DesiredObject.transform.position = transform.position;
            //_DesiredObject.GetComponent<Pushable>().enabled = true;
            SwitchOn(true);
            //DoAnimateOn();
            _triggered = true;
        }
        else
        {
            Debug.Log("Trigger left!");
            SwitchOn(false);
            _triggered = false;
        }
    }

    //private void DoAnimateOn()
    //{
    //    foreach (var item in _AnimationsToTurnOn)
    //    {
    //        item.GetComponent<Animator>().SetTrigger("Open");
    //    }
    //}
    //private void DoAnimateOff()
    //{
    //    foreach (var item in _AnimationsToTurnOn)
    //    {
    //        item.GetComponent<Animator>().SetTrigger("Close");
    //    }
    //}

    public void SwitchOn(bool isTurnedOn)
    {
        foreach (var item in _ObjectsToActivate)
        {
            item.SetActive(isTurnedOn);         
        }
        foreach (var item in _ObjectsToDisable)
        {
            item.SetActive(!isTurnedOn);
        }

    }




}
