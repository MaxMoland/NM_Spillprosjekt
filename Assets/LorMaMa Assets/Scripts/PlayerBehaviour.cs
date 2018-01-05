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
        Pushing, Attacking, Idle, Dead, EnterSymetry, SymetryPhasing
    }
    private State _state = State.Idle;
    private State _lastState = State.Idle;
    private bool _isEnteringState = false;

    //component refs
    private NavMeshAgent _NMAgent;
    private AudioSource _Speaker;

    //Sounds
    [Header("Sounds")]
    public AudioClip _phaseloop;

    Vector3 _PhaseLinePoint = Vector3.zero; //used by entersymetry state
    GameObject _PhaseLineObject;    //used by symetryPhasing
    float _phasingInterpolationPos;  //used by symetryPhasing


    //Setting up varables, awake is called before Start()
    private void Awake()
    {
        _NMAgent = GetComponent<NavMeshAgent>();
        _Speaker = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        _NMAgent.destination = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //State Machine
        Debug.Log("Player State is: " + _state);
        if (_state != _lastState) //detect state transition
        {
            _isEnteringState = true;
            _lastState = _state;
        }
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
            //State variables
            _NMAgent.enabled = false;
            if (_Speaker.clip != _phaseloop)
            {
            _Speaker.clip = _phaseloop;
            _Speaker.Play();
            _Speaker.loop = true;
            }
            //move to phase line Todo: use _isEntering to detect first run.
            if (_PhaseLinePoint == Vector3.zero)
            {_PhaseLinePoint = ClosestPhaseLinePoint(); }
            if (_PhaseLinePoint != Vector3.zero)
            {
                transform.position = Vector3.MoveTowards(transform.position, _PhaseLinePoint, 0.1f);
                //Debug.Log("Moves toward a point!:");
                Debug.DrawLine(transform.position, _PhaseLinePoint, Color.red);
            }
            //Exit state -> Idle
            if (Input.GetButtonUp("Jump"))
            {
                _state = State.Idle;
                //cleans up on exit
                _Speaker.clip = null;
                _PhaseLinePoint = Vector3.zero; 
            }
            //Exit state -> SymetryPhasing
            if (Vector3.Distance(transform.position,_PhaseLinePoint) < 0.2f)
            {
                _state = State.SymetryPhasing;
               // _PhaseLineObject = 
                //cleans up on exit
                _PhaseLinePoint = Vector3.zero;
            }

        }
        if (_state == State.SymetryPhasing)
        {
            //find current phase line
            if (_isEnteringState)
            {
                //find the phase line currently attached to
                _PhaseLineObject = ClosestSymetryLine();
                _phasingInterpolationPos = PhasingInterpolationPos(_PhaseLineObject);
                Debug.Log("Players interpolation value is: " + _phasingInterpolationPos);
            }
            SymetryNavigate(_PhaseLineObject);
            //Exit state -> Idle
            if (Input.GetButtonUp("Jump"))
            {
            //todo: exit logic death or not allowed?
                _state = State.Idle;
                //cleans up on exit
                _Speaker.clip = null;
            }
        }
        _isEnteringState = false; //reset entering state to false
        
    }


    /// <summary>
    /// The player is allowed to move using normal controlls
    /// </summary>
    private void Navigate()
    {
        //Sets a destination for the nav mesh agent to navigate to, 
        //how the agent actually gets from point a to point b is handled by the agent

        Vector3 destination = transform.position; //destination is the same as current position.
        destination.x += Input.GetAxisRaw("Horizontal"); //Offset from current position based on input
        destination.z += Input.GetAxisRaw("Vertical");
        _NMAgent.destination = destination;
        //Debug.Log("Player tries to navigate");

    }

    /// <summary>
    /// The player is allowed to move along phase line
    /// </summary>
    private void SymetryNavigate(GameObject SymetryLine)
    {
        
    }

    /// <summary>
    /// Searches nearby for phase line points, if none is found, returns Vector3.zero.
    /// </summary>
    /// <returns></returns>
    private Vector3 ClosestPhaseLinePoint()
    {
        Vector3 closestPhasePoint;

        //Find potential active phase points

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1);
        List<Vector3> PhasePoints = new List<Vector3>();
        foreach (var item in hitColliders)
        {
            if (item.tag == "PhaseLine") {
                PhasePoints.Add(item.transform.position);
                //Debug.Log("removed "+ item.gameObject.name + " from list ");
            }
        }
        if (PhasePoints.Any() == false) {
            Debug.Log("No phase point within reach!");
            return Vector3.zero; //No phase point within reach!            
        }
        closestPhasePoint = PhasePoints[0];
        foreach (Vector3 point in PhasePoints)
        {
            if (Vector3.Distance(transform.position, point) < Vector3.Distance(transform.position, closestPhasePoint))
            {closestPhasePoint = point;} //Determine closest point
        }
        return closestPhasePoint;
    }
    /// <summary>
    /// returns the phase line object that is closest to the player.
    /// </summary>
    /// <returns></returns>
    private GameObject ClosestSymetryLine()
    {
        GameObject closestPhasePoint;

        //Find potential active phase points

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.3f);
        List<GameObject> PhasePoints = new List<GameObject>();
        foreach (var item in hitColliders)
        {
            if (item.tag == "PhaseLine")
            {
                PhasePoints.Add(item.gameObject);
            }
        }          
        closestPhasePoint = PhasePoints[0];
        foreach (GameObject point in PhasePoints)
        {
            if (Vector3.Distance(transform.position, point.transform.position) < Vector3.Distance(transform.position, closestPhasePoint.transform.position))
            { closestPhasePoint = point; } //Determine closest point
        }
        return closestPhasePoint.transform.parent.parent.parent.gameObject; //return great-grandparent of line point.
    }
    /// <summary>
    /// Where on an a phasing line is the player?
    /// </summary>
    /// <param name="phaseLineObject"></param>
    /// <returns></returns>
    private float PhasingInterpolationPos(GameObject phaseLineObject)
    {
        Vector3 A = phaseLineObject.transform.Find("A").transform.position;
        Vector3 B = phaseLineObject.transform.Find("B").transform.position;
        float distToA = Vector3.Distance(transform.position, A);
        float distAtoB = Vector3.Distance(A, B);
        return distToA/distAtoB;
    }
}
