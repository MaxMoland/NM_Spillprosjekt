using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

public class PlayerBehaviour : MonoBehaviour
{

    public enum State
    {
        Pushing, Attacking, Idle, Dead, EnterSymetry
    }
    private State _state = State.Idle;

    //component refs
    private NavMeshAgent _NMAgent;

    //Sounds
    [Header("Sounds")]
    public AudioClip _phaseloop;

    Vector3 _PhaseLinePoint = Vector3.zero; //used by enterPhase state


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
    void Start()
    {
        _NMAgent.destination = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
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
            
            if (_PhaseLinePoint == Vector3.zero)
            {_PhaseLinePoint = ClosestPhaseLinePoint(); }
            if (_PhaseLinePoint != Vector3.zero)
            {
                transform.position = Vector3.MoveTowards(transform.position, _PhaseLinePoint, 0.1f);
                Debug.Log("Moves toward a point!:");
                Debug.DrawLine(transform.position, _PhaseLinePoint, Color.red);
            }
            //Exit state
            if (Input.GetButtonUp("Jump"))
            {
                _state = State.Idle;
                _PhaseLinePoint = Vector3.zero; //cleans up on exit
            }

        }

    }
    /// <summary>
    /// Searches nearby for phase line points, if none is found, returns Vector3.zero.
    /// </summary>
    /// <returns></returns>
    private Vector3 ClosestPhaseLinePoint()
    {
        Vector3 closestPhasePoint;

        //Find potential active phase points

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4);
        List<Vector3> PhasePoints = new List<Vector3>();
        foreach (var item in hitColliders)
        {
            if (item.tag == "PhaseLine") PhasePoints.Add(item.transform.position);
        }
        if (PhasePoints.Any() == false) {
            Debug.Log("No phase point within reach!");
            return Vector3.zero; //No phase point within reach!            
        }
        closestPhasePoint = hitColliders[0].transform.position;
        foreach (Vector3 point in PhasePoints)
        {
            if (Vector3.Distance(transform.position, point) < Vector3.Distance(transform.position, closestPhasePoint))
            {closestPhasePoint = point;} //Determine closest point
        }
        return closestPhasePoint;
    }
}
