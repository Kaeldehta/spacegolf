using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMainMenuUI : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.ONMainMenuEnter += () => gameObject.SetActive(true);
        GameManager.Instance.ONLevelSelectEnter += () => gameObject.SetActive(false);
        GameManager.Instance.ONHowToEnter += () => gameObject.SetActive(false);
    }
}
