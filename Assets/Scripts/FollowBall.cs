using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowBall : MonoBehaviour
{

    void LateUpdate()
    {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.BallMovement) return;

        transform.position = GameStateManager.Instance.Planet.transform.position + (GameStateManager.Instance.GolfBall.transform.position - GameStateManager.Instance.Planet.transform.position).normalized * 15f;

        transform.LookAt(GameStateManager.Instance.GolfBall.transform);


    }
}
