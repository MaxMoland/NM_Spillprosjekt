using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SymetryZoneLogic : MonoBehaviour {

    public SimpleTrigger[] _requiredActiveTriggers;
    public GameObject[] _ObjectsToActivate;
    public GameObject[] _ObjectsToDisable;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool allTrue = true;
        foreach (var item in _requiredActiveTriggers)
        {
            if (!item._triggered)
            {
                allTrue = false;
                Debug.Log("There is a false trigger in the scene!");
            }
        }
        if (allTrue)
        {
            SwitchOn();
        }
        else SwitchOff();
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
