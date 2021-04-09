using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Golfable : MonoBehaviour
{

    private Rigidbody _rigidbody;
    
    private float _timer;
    private bool _updateTimer;

    private Vector3 _originPos;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        GameStateManager.Instance.ONBallMovementEnter += initVelocity =>
        {
            _originPos = transform.position;
            _rigidbody.velocity = initVelocity;
            _timer = 0;
        };
        
        GameStateManager.Instance.ONInputEnter += () => _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        _updateTimer = false;
        _timer = 0;
    }

    private void OnCollisionExit(Collision other)
    {
        _updateTimer = true;
    }

    private void LateUpdate()
    {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.BallMovement) return;
        
        if(_updateTimer) _timer += Time.deltaTime;
        
        if (_rigidbody.velocity.sqrMagnitude < 0.07f) GameStateManager.Instance.StartInput();
        else if (_timer >= 5f)
        {
            transform.position = _originPos;
            
            GameStateManager.Instance.StartInput();
        }

        
    }
}
