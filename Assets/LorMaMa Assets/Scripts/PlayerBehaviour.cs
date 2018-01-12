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
    public SpriteRenderer _Sprite;
    public Sprite[] _Sprites;
    //component refs
    private NavMeshAgent _NMAgent;
    private AudioSource _Speaker;

    //Sounds
    [Header("Sounds")]
    public AudioClip _phaseloop;

    Vector3 _PhaseLinePoint = Vector3.zero; //used by entersymetry state
    GameObject _PhaseLineObject;    //used by symetryPhasing
    [Tooltip("units per second")]
    public float symetrySpeed = 1;
    private bool _IsMovingToB;
    private float _Pushcount = 0;
    [NonSerialized]
    public Vector3 _heading;
    [NonSerialized]
    public Vector3 _lastHeading;
    private GameObject _pushableObject;
    public float _Strenght = 0;
    private int count = 0;

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
    void Update()
    {
            _lastHeading = _heading;
            _heading = Vector3.zero;
            _heading.x += Mathf.Round(Input.GetAxisRaw("Horizontal")); //Offset from current position based on input to get heading
            _heading.z += Mathf.Round(Input.GetAxisRaw("Vertical"));
        if (count == 20)
            count = 0;
        count++;

        //State Machine -------------------------------------------------------------------------------------------
        Debug.Log("Player State is: " + _state);
        if (_state != _lastState) //detect state transition
        {
            _isEnteringState = true;
            _lastState = _state;
        }
        if (_state == State.Idle)
        {

            if (_heading.z < 0)//down
            {
                if (count <= 10)
                    _Sprite.sprite = _Sprites[8];
                else
                    _Sprite.sprite = _Sprites[7];//8
            }
            else if (_heading.z > 0) //up
            {
                if (count <= 10)
                    _Sprite.sprite = _Sprites[2];
                else
                    _Sprite.sprite = _Sprites[1]; //2
            }
            else if (_heading.x < 0) //left
            {
                if (count <= 10)
                    _Sprite.sprite = _Sprites[5];
                else
                    _Sprite.sprite = _Sprites[4]; //5
                _Sprite.flipX = true;
            }
            else if (_heading.x > 0) //right
            {
                if (count <= 10)
                    _Sprite.sprite = _Sprites[5];
                else
                _Sprite.sprite = _Sprites[4]; //5  


                _Sprite.flipX = false;
            }
            else
            {
                if (_lastHeading.z < 0)//down
                {
                    _Sprite.sprite = _Sprites[6];
                }
                else if (_lastHeading.z > 0) //up
                {
                    _Sprite.sprite = _Sprites[0];
                }
                else if (_lastHeading.x < 0) //left
                {
                    _Sprite.sprite = _Sprites[3];
                    _Sprite.flipX = true;
                }
                else if (_lastHeading.x > 0) //right
                {
                    Debug.Log("here");
                    _Sprite.sprite = _Sprites[3];
                    _Sprite.flipX = false;
                }
              
            }

            _NMAgent.enabled = true;

            Navigate();
            if (Input.GetButton("Jump"))
            {
                _state = State.EnterSymetry;
            }

            //pushing is the player pushing agains a pushable object? 
            //TODO: Add "rake-cast" for better accuracy
            Debug.DrawRay(transform.position, _heading * 0.5f, Color.green, 0.2f);
            Ray ray = new Ray(transform.position, _heading);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.5f))
            {
                if (hit.collider.GetComponent<Pushable>() != null)
                {
                    print("There is something pushablle in front of the object!");
                    _Pushcount = _Pushcount + 1 * Time.deltaTime;
                    //Debug.Log(_Pushcount);
                    transform.position = hit.collider.transform.position - _heading;
                    _NMAgent.destination = transform.position; //don't move closer to object
                }
            }
            else _Pushcount = 0;
            if (_Pushcount > 1/_Strenght) //how long should play strain before starting to push?
            {
                _state = State.Pushing;
                _Pushcount = 0;
                _pushableObject = hit.collider.gameObject;
            }
            

        }
        if (_state == State.EnterSymetry)
        {
            _Sprite.sprite = _Sprites[14];//15

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
                transform.position = Vector3.MoveTowards(transform.position, _PhaseLinePoint, 1f * Time.deltaTime);
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
                _Sprite.sprite = _Sprites[6];
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
            if (_isEnteringState)
            {
                _Sprite.sprite = _Sprites[14];//15
                Debug.Log("entered SymetryPhasing!");
                //find the phase line currently attached to
                _PhaseLineObject = ClosestSymetryLine();
                _IsMovingToB = PhasingInterpolationPos(_PhaseLineObject) < 0.5;

            }
            SymetryNavigate(_PhaseLineObject);
            //Exit state -> Idle
            if (Input.GetButtonUp("Jump")) //todo: exit logic death or not allowed?           
            {
                _state = State.Idle;
                //cleans up on exit
                _Speaker.clip = null;
                _Sprite.sprite = _Sprites[6];
            }
        }
        if (_state == State.Pushing)
        {
 
            _NMAgent.enabled = false;

            if (_pushableObject.GetComponent<Pushable >().IsPushable(this))
            {
                Vector3 objectOffset = transform.position - _pushableObject.transform.position;
                float speed = _NMAgent.speed / _pushableObject.GetComponent<Pushable>()._mass;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + _heading, speed * Time.deltaTime);
                _pushableObject.transform.position = transform.position - objectOffset;
                if (_heading.z < 0) { //down
                   _Sprite.sprite = _Sprites[13];
                }
                else if(_heading.z > 0) //up
                {
                    if (count <= 10)
                        _Sprite.sprite = _Sprites[10];
                    else
                        _Sprite.sprite = _Sprites[9]; //10
                }
                else if (_heading.x < 0) //left
                {
                    if (count <= 10)
                        _Sprite.sprite = _Sprites[12];
                    else
                        _Sprite.sprite = _Sprites[11]; //12
                    _Sprite.flipX = true;
                }
                else if (_heading.x > 0) //right
                {
                    if (count <= 10)
                        _Sprite.sprite = _Sprites[11];
                    else
                        _Sprite.sprite = _Sprites[12]; //11   
                    _Sprite.flipX = false;
                }

            } else _state = State.Idle;
            if (_heading != _lastHeading)
            {

                _state = State.Idle;
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
    /// The player is allowed to move along phase line
    /// </summary>
    private void SymetryNavigate(GameObject SymetryLine)
    {
        Vector3 A = SymetryLine.transform.Find("A").transform.position;
        Vector3 B = SymetryLine.transform.Find("B").transform.position;
        Debug.Log("IsmovingtoB = " + _IsMovingToB);
        if (_IsMovingToB) { transform.position = Vector3.MoveTowards(transform.position, B, symetrySpeed * Time.deltaTime); }
        else { transform.position = Vector3.MoveTowards(transform.position, A, symetrySpeed * Time.deltaTime); } 
    }
  // /// <summary>
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
        Debug.Log(distToA / distAtoB);
        return distToA/distAtoB;
   }

    public State GetState() { return _state; }

}
