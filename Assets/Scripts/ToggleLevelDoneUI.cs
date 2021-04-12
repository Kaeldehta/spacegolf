using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLevelDoneUI : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.ONLevelCompleted += () => gameObject.SetActive(true);
        GameManager.Instance.ONMainMenuEnter += () => gameObject.SetActive(false);
    }
}
