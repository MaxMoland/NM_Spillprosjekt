using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

public class PlayerBehaviour : MonoBehaviour {

    public enum State
    {
        Pushing, Attacking, Idle, Dead
    }

    private State _state = State.Idle;
    private NavMeshAgent _NMAgent;
    //Setting up varables
    private void Awake()
    {
        _NMAgent = GetComponent<NavMeshAgent>();
    }

    private void Navigate()
    {
        //Sets a destination for the nav mesh agent to navigate to, 
        //how the agent actually gets from point a to point b is handled by the agent

        Vector3 destination = transform.position; //destination is the same as current position.
        destination.x += Input.GetAxisRaw("Horizontal"); //Offset from current position based on input
        destination.z += Input.GetAxisRaw("Vertical");
        _NMAgent.destination = destination;
        Debug.Log("Player tries to navigate");

    }


    // Use this for initialization
    void Start () {
        _NMAgent.destination = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if (_state == State.Idle)
        {
            Navigate();
        }
		
	}
}
