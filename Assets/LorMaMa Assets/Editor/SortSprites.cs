using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SortSprites
{
    [MenuItem("LorMaMa/Reorder Sprites in scene")]
    private static void Reorder()
    {
        float worldZ;
        SpriteRenderer[] Sprites = GameObject.FindObjectsOfType<SpriteRenderer>();
        foreach (var item in Sprites)
        {
            //worldZ = item.gameObject.transform.TransformPoint(item.gameObject.transform.position).z * -100;
            worldZ = item.gameObject.transform.position.z * -100;
            item.sortingOrder = (int)worldZ;
            if (item.gameObject.layer == 9) //ground
            {
                item.sortingOrder -= 1000;
                //Debug.DrawRay(item.transform.position, Vector3.up);
                Debug.DrawLine(item.transform.position,item.transform.position + Vector3.up * item.gameObject.transform.position.z, Color.white, 10);
            }
        }
    }
}
