using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApplyGravity : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private Rigidbody[] _targets;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _targets = FindObjectsOfType<Rigidbody>().Where(obj => obj.CompareTag("Planet")).ToArray();
    }

    private void FixedUpdate()
    {
        // Iterate over each target to calculate gravitational force for that target.
        foreach (var target in _targets)
        {
            // Calculate vector between this object and target
            Vector3 between = target.transform.position - transform.position;
            
            // Calculate the acceleration that needs to be applied to this GameObject
            Vector3 acceleration = GameSettings.Instance.GravitationalConstant  * target.mass /
                between.sqrMagnitude * between.normalized;
            
            // Apply acceleration to rigidbody
            _rigidbody.AddForce(acceleration, ForceMode.Acceleration);

        }
    }
}
