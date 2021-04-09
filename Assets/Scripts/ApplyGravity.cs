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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        GameStateManager.Instance.ONLevelStartExit += () => {
            Debug.Log("ApplyGrav");
            _planetRb = GameStateManager.Instance.Planet.GetComponent<Rigidbody>();
         };
    }

    private void FixedUpdate()
    {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.BallMovement) return;

        // Calculate vector between this object and target
        Vector3 between = _planetRb.transform.position - transform.position;

        // Calculate the acceleration that needs to be applied to this GameObject
        Vector3 acceleration = GameSettings.Instance.GravitationalConstant * _planetRb.mass /
            between.sqrMagnitude * between.normalized;

        // Apply acceleration to rigidbody
        _rigidbody.AddForce(acceleration, ForceMode.Acceleration);
    }
}
