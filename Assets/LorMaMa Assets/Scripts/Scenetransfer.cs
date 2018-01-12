using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenetransfer : MonoBehaviour {

    public string scene;
    public Color loadToColor = Color.white;
    public GameObject _TriggerObj;


    private void Update()
    {
        if (Vector3.Distance(transform.position, _TriggerObj.transform.position) < 0.8f)
        {
        Debug.Log("Trigger entered!");
        Triggered();
        }
    }


    public void Triggered()
    {
        Initiate.Fade(scene, loadToColor, 2.0f);
    }
}
