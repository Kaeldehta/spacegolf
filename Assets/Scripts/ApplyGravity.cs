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
        _rigidbody = GetComponent<Rigidbody>();

        handler1 = () => {
            _planetRb = GameManager.Instance.Planet.GetComponent<Rigidbody>();
        };

        GameManager.Instance.ONLevelStartExit += handler1;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ONLevelStartExit -= handler1;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.BallMovement) return;

        // Calculate vector between this object and target
        Vector3 between = _planetRb.transform.position - transform.position;

        // Calculate the acceleration that needs to be applied to this GameObject
        Vector3 acceleration = GameManager.Instance.GravitationalConstant * _planetRb.mass /
            between.sqrMagnitude * between.normalized;

        // Apply acceleration to rigidbody
        _rigidbody.AddForce(acceleration, ForceMode.Acceleration);
    }
}
