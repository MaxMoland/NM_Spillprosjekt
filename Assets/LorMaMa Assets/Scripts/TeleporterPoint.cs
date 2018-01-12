using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPoint : MonoBehaviour {

    public GameObject PortableObjectA;
    public GameObject PortableObjectB;
    public GameObject PortPoint;
    private bool Active = false;
    private bool AOnPlate = false;
    private bool BOnPlate = false;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, PortableObjectA.transform.position) < 0.8f)
        {
            Active = true;
            AOnPlate = true;
            BOnPlate = false;
        }
        else if (Vector3.Distance(transform.position, PortableObjectB.transform.position) < 0.8f)
        {
            Active = true;
            BOnPlate = true;
            AOnPlate = false;
        }
        else {
            Active = false;
            AOnPlate = false;
            BOnPlate = false;
        }
    }

    public bool Ready()
    {
        if (Active)
            return true;
        return false;
    }

    public void Teleport() {
        if (AOnPlate)
        {
            PortableObjectA.transform.position = PortPoint.transform.position;
        }
        else if (BOnPlate)
        {
            PortableObjectB.transform.position = PortPoint.transform.position;
        }
    }
}
