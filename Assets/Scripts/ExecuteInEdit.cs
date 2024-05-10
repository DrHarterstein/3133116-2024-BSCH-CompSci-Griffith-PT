using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExecuteInEdit : MonoBehaviour
{
    void Update()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = new Vector3(transform.GetChild(i - 1).position.x - 10, 0, 0);
            transform.GetChild(i).name = "Obstacle" + i;
        }
    }
}
