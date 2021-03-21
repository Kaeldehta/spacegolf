using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    private static GameSettings _settingsSingleton;

    public static GameSettings Instance => _settingsSingleton;

    [SerializeField] private float gravitationalConstant;

    public float GravitationalConstant => gravitationalConstant;

    void Start()
    {
        if (_settingsSingleton == null)
        {
            _settingsSingleton = this;
        }
    }

}
