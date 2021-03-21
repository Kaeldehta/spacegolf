using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golfable : MonoBehaviour
{

    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Stop();
    }


    public void Stop()
    {
        _rigidbody.isKinematic = true;
    }

    public void Hit(Vector3 initialVelocity)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(initialVelocity, ForceMode.Impulse);
    }
}
