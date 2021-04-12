using System;
using UnityEngine;

public class GolfBall : MonoBehaviour
{

    // reference to golfballs rigidbody
    private Rigidbody _rigidbody;
    
    // timer to check how long golfball didnt collide with anything
    private float _timer;
    private bool _updateTimer;

    // last position from which ball has been shot
    private Vector3 _originPos;

    // event handlers
    private Action<Vector3> handler1;
    private Action handler2;

    private void Awake()
    {
        // get rigidbody
        _rigidbody = GetComponent<Rigidbody>();

        // handler that sets origin pos, applies hit velocity and starts timer
        handler1 = initVelocity =>
        {
            _originPos = transform.position;
            _rigidbody.velocity = initVelocity;
            _timer = 0;
        };

        GameManager.Instance.ONBallMovementEnter += handler1;

        // handler that resets velocity and angular velocity
        handler2 = () =>
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        };

        GameManager.Instance.ONInputEnter += handler2;
    }

    private void OnDestroy()
    {
        // deregister both handlers when gameobject is destroyed
        GameManager.Instance.ONBallMovementEnter -= handler1;
        GameManager.Instance.ONInputEnter -= handler2;
    }


    private void OnCollisionEnter(Collision other)
    {
        // stop timer updates when entering collision
        _updateTimer = false;
    }

    private void OnCollisionExit(Collision other)
    {
        // continue timer updates when exiting collision
        _updateTimer = true;
        _timer = 0;
    }

    private void LateUpdate()
    {
        // if not in ball movement state ball is not moving
        if (GameManager.Instance.CurrentState != GameManager.GameState.BallMovement) return;
        
        // update timer if allowed
        if(_updateTimer) _timer += Time.deltaTime;
        
        // go to input if ball stopped moving
        if (_rigidbody.velocity.sqrMagnitude < 0.07f) GameManager.Instance.StartInput();
        // if ball in air for more than 5 sec reset ball and start input
        else if (_timer >= 5f)
        {
            transform.position = _originPos;
            
            GameManager.Instance.StartInput();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        // reset ball if it hits water
        if (other.CompareTag("Water"))
        {
            transform.position = _originPos;

            GameManager.Instance.StartInput();
        }
        // complete level if ball in goal
        else if (other.CompareTag("Goal"))
        {
            GameManager.Instance.CompleteLevel();
        }
    }
}
