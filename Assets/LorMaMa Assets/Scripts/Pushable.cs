using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author Mattias Tronslien
/// mntronslien@gmail.com
/// 2018
/// 
/// Use this script on objects that should be pushable
/// </summary>
public class Pushable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Returns true if the object can be pushed away from pushingEntity
    /// </summary>
    /// <param name="pushingEntity"> The entity who does the pushing</param>
    /// <returns></returns>
    public bool IsPushable(GameObject pushingEntity)
    {
        return true;
    }
}


