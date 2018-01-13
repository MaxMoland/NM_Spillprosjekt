using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour {
    public string _URL = "http://www.mattiastronslien.com/TeamAvA.jpg";


    public void Open()
    {
        Application.OpenURL(_URL);
    }
    public void Open(string url)
    {
        Application.OpenURL(url);
    }
}

