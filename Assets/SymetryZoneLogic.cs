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
        bool allTrue = true;
        foreach (var item in _requiredActiveTriggers)
        {
            if (!item._triggered)
            {
                allTrue = false;
            }
        }
        if (allTrue)
        {
            SwitchOn();
        }
        else SwitchOff();
        Debug.Log("_tempcheck = " + _tempcheck);
        Debug.Log("_requiredActiveTriggers.Length = " + _requiredActiveTriggers.Length);
    }

    public void SwitchOn()
    {
        foreach (var item in _ObjectsToActivate)
        {
            item.SetActive(true);
        }
    }
    public void SwitchOff() {
        foreach (var item in _ObjectsToDisable)
        {
            item.SetActive(false);
        }
    }
}
