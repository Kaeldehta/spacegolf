using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.ONMainMenuEnter += () => gameObject.SetActive(true);
        GameManager.Instance.ONLevelStartExit += () => gameObject.SetActive(false);
    }
}
