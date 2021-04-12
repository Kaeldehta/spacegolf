using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLevelSelectionUI : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.ONMainMenuEnter += () => gameObject.SetActive(false);
        GameManager.Instance.ONLevelSelectEnter += () => gameObject.SetActive(true);
        GameManager.Instance.ONLevelStartExit += () => gameObject.SetActive(false);
    }
}
