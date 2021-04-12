using System;
using UnityEngine;

public class GolfBall : MonoBehaviour
{

    private Rigidbody _rigidbody;
    
    private float _timer;
    private bool _updateTimer;

    private Vector3 _originPos;

    private Action<Vector3> handler1;
    private Action handler2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        handler1 = initVelocity =>
        {
            _originPos = transform.position;
            _rigidbody.velocity = initVelocity;
            _timer = 0;
        };

        GameManager.Instance.ONBallMovementEnter += handler1;

        handler2 = () =>
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        };

        GameManager.Instance.ONInputEnter += handler2;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ONBallMovementEnter -= handler1;
        GameManager.Instance.ONInputEnter -= handler2;
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
        if (GameManager.Instance.CurrentState != GameManager.GameState.BallMovement) return;
        
        if(_updateTimer) _timer += Time.deltaTime;
        
        if (_rigidbody.velocity.sqrMagnitude < 0.07f) GameManager.Instance.StartInput();
        else if (_timer >= 5f)
        {
            transform.position = _originPos;
            
            GameManager.Instance.StartInput();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            transform.position = _originPos;

            GameManager.Instance.StartInput();
        }
        else if (other.CompareTag("Goal"))
        {
            GameManager.Instance.CompleteLevel();
        }
    }
}
