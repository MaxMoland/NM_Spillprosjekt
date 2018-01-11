using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SortSprites
{
    [MenuItem("LorMaMa/'Reorder Sprites in scene")]
    private static void Reorder()
    {
        SpriteRenderer[] Sprites = GameObject.FindObjectsOfType<SpriteRenderer>();
        foreach (var item in Sprites)
        {
            item.sortingOrder = (int)item.gameObject.transform.position.z * -1;
        }
    }
}
