using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderUpdater : MonoBehaviour {

    private SpriteRenderer _Render;

	// Use this for initialization
	void Start () {
        _Render = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        _Render.sortingOrder = (int)gameObject.transform.position.z * -1;
    }
}
