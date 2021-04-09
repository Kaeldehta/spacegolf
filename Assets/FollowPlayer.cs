using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowPlayer : MonoBehaviour
{


    [SerializeField] private float upOffset = 1f;
    [SerializeField] private float forwardOffset = 3f;

    private void LateUpdate()
    {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.Input) return;

        //ansform.position = _player.transform.position - _player.transform.forward * forwardOffset + _player.transform.up * upOffset;
        
        //transform.rotation = Quaternion.LookRotation(_player.transform.position - transform.position, _player.transform.forward);
        
    }
}
