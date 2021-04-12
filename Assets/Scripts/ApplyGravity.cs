using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApplyGravity : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private Rigidbody _planetRb;

    private Action handler1;

    private void Awake()
    {
        // get the rigidbody for use during fixed update
        _rigidbody = GetComponent<Rigidbody>();

        // add handler to level start exit event that fetches the planets rigidbody
        handler1 = () => {
            _planetRb = GameManager.Instance.Planet.GetComponent<Rigidbody>();
        };

        GameManager.Instance.ONLevelStartExit += handler1;
    }

    private void OnDestroy()
    {
        // deregister handler when object gets destroyed
        GameManager.Instance.ONLevelStartExit -= handler1;
    }

    private void FixedUpdate()
    {
        // Gravity should only be applied in ball movement state
        if (GameManager.Instance.CurrentState != GameManager.GameState.BallMovement) return;

        // Calculate vector between golfball and planet
        Vector3 between = _planetRb.transform.position - transform.position;

        // Calculate the acceleration that needs to be applied to golfball
        Vector3 acceleration = GameManager.Instance.GravitationalConstant * _planetRb.mass /
            between.sqrMagnitude * between.normalized;

        // Apply acceleration to rigidbody
        _rigidbody.AddForce(acceleration, ForceMode.Acceleration);
    }
}
