using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLogic : MonoBehaviour {


    public SimpleTrigger _plate;
    private Animator _anim;

    private bool isOpen = false;


    // Use this for initialization
    void Start () {
        _anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (_plate._triggered && !isOpen)
        {
            _anim.SetTrigger("Open");
            isOpen = !isOpen;
        }
        if (!_plate._triggered && isOpen)
        {
            _anim.SetTrigger("Open");
            isOpen = !isOpen;
        }

    }
}
