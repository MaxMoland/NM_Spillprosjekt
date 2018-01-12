using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSwitch : MonoBehaviour
{

    public GameObject PointA;
    public GameObject PointB;
    public GameObject _DesiredObject;
    public Color colorActive;
    public Color colorDeActive;
    bool Active = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < 0.8f)
        {
            GetComponent<Renderer>().material.color = colorActive;
            transform.localScale = new Vector3(0.8f, 0.05f, 0.8f);
            if (PointA.GetComponent<TeleporterPoint>().Ready() || PointB.GetComponent<TeleporterPoint>().Ready())
            {
                if (!Active)
                {
                    Debug.Log("teleport");
                    Active = true;
                    Teleport();
                }
            }
            else {
                //error lyd
            }
            
        }
        else if (Vector3.Distance(transform.position, _DesiredObject.transform.position) > 0.8f)
        {
            GetComponent<Renderer>().material.color = colorDeActive;
            transform.localScale = new Vector3(0.8f, 0.1f, 0.8f);
            Active = false;
        }

       
            
    }
    void Teleport()
    {
        PointA.GetComponent<TeleporterPoint>().Teleport();
        PointB.GetComponent<TeleporterPoint>().Teleport();
    }
}