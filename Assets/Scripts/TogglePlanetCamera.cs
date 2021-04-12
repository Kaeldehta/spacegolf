using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TogglePlanetCamera : MonoBehaviour
{
    private Action handler1;
    private Action handler2;


    private void Awake()
    {
        handler1 = () => gameObject.SetActive(false);
        GameManager.Instance.ONMainMenuEnter += handler1;
        handler2 = () => gameObject.SetActive(true);
        GameManager.Instance.ONLevelStartExit += handler2;
    }

    private void OnDestroy()
    {
        GameManager.Instance.ONMainMenuEnter -= handler1;
        GameManager.Instance.ONLevelStartExit -= handler2;
    }
}
