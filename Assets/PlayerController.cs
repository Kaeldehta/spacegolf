using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Golfable _golfable;
    
    void Start()
    {
        _golfable = FindObjectOfType<Golfable>();
    }

}
