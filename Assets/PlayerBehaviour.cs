using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

public class PlayerBehaviour : MonoBehaviour {

    public enum State
    {
        Pushing, Attacking, Idle, Dead, EnterSymetry
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
        Debug.Log("Player State is: " + _state);
        if (_state == State.Idle)
        {
            _NMAgent.enabled = true;

            Navigate();
            if (Input.GetButton("Jump"))
            {
                _state = State.EnterSymetry;
            }
        }
        if (_state == State.EnterSymetry)
        {
            _NMAgent.enabled = false;

            //TODO: Make methods for Enter Symetry state
            Vector3 closestPhasePoint;
            //Find potential active symetry lines
            //List<Collider> hitColliders = Physics.OverlapSphere(transform.position, 2000, 8, QueryTriggerInteraction.Collide).ToList();
            List<Collider> hitColliders = Physics.OverlapSphere(transform.position, 2000).ToList();
            Debug.Log(hitColliders.Count);
            //if any points are within range, find the closest
            if (hitColliders.Count > 0)
            {
                closestPhasePoint = hitColliders[0].transform.position;
                foreach (Collider item in hitColliders)
                {
                    if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, closestPhasePoint) && item.tag == "PhaseLine")
                    {
                        closestPhasePoint = item.transform.position;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, closestPhasePoint, 1);
                Debug.Log("Moves toward a point!:" + closestPhasePoint);
            }
            //Exit state
            if (Input.GetButtonUp("Jump"))
            {
                _state = State.Idle;
            }

        }
		
	}
}
