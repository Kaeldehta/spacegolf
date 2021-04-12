using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlignToParent : MonoBehaviour
{

#if UNITY_EDITOR
    void Update()
    {
        // Align transform to parent in edit mode
        transform.up = transform.position - transform.parent.position;
    }
#endif
}
