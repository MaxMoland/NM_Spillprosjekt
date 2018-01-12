using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymetryZoneLogic : MonoBehaviour {

    public SimpleTrigger[] _requiredActiveTriggers;
    private float _tempcheck;
    public GameObject[] _ObjectsToActivate;
    public GameObject[] _ObjectsToDisable;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _tempcheck = 0;
        foreach (var item in _requiredActiveTriggers)
        {
            if (item._triggered)
            {
                _tempcheck++;
            }
        }
        if (_tempcheck == _requiredActiveTriggers.Length)
        {
            SwitchOn(true);
        }
        else SwitchOn(false);
	}
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
