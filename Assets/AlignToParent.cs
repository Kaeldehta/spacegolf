using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlignToParent : MonoBehaviour
{

    void Update()
    {
        transform.up = transform.position - transform.parent.position;
    }
}
