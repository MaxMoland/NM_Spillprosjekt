using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The only job of the drigger class is to tell the wold it has been triggered
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SimpleTrigger : MonoBehaviour {

    protected PlayerBehaviour _playerBehaviour;
    public GameObject _DesiredObject;
    public AudioClip _audioClip;

    public bool _triggered = false;
    public float _SnapRange = 0.8f;

    protected virtual void Start()
    {
        _playerBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) < _SnapRange && _playerBehaviour.GetState() != PlayerBehaviour.State.Pushing && !_triggered)
        {

            GetComponent<AudioSource>().PlayOneShot(_audioClip);
            _DesiredObject.transform.position = transform.position;
            _triggered = true;
        }
        if (Vector3.Distance(transform.position, _DesiredObject.transform.position) > _SnapRange)
        {
            _triggered = false;
        }
    }
}
