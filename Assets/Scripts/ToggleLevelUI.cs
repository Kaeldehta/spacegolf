using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLevelUI : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.ONLevelStartExit += () => gameObject.SetActive(true);
        GameManager.Instance.ONMainMenuEnter += () => gameObject.SetActive(false);
    }
}
