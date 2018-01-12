using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PresurePlate : SimpleTrigger {


    public Color colorActive;
    public Color colorDeActive;
    private Material[] mat;
    

    protected override void Start()
    {
        base.Start();
        mat = GetComponent<Renderer>().materials;
    }

    protected override void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < _SnapRange && base._playerBehaviour.GetState() != PlayerBehaviour.State.Pushing && !base._triggered)
        {
            if (!_triggered)
            {
                GetComponent<AudioSource>().PlayOneShot(_audioClip);
            }
            _DesiredObject.transform.position = transform.position + (Vector3.up * 0.5f);
            GetComponent<Renderer>().material.color = colorActive;
            _triggered = true;
        }
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) > _SnapRange)
        {
            _triggered = false;
            GetComponent<Renderer>().material.color = colorDeActive;
        }


    }
}
