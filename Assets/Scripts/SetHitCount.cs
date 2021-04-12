using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetHitCount : MonoBehaviour
{

    private void Awake()
    {
        var text = GetComponent<TextMeshProUGUI>();
        GameManager.Instance.ONLevelStartExit += () => text.text = GameManager.Instance.NumberOfHits.ToString();
        GameManager.Instance.ONBallMovementEnter += velo => text.text = GameManager.Instance.NumberOfHits.ToString();
    }
}
