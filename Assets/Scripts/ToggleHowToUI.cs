using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHowToUI : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.ONMainMenuEnter += () => gameObject.SetActive(false);
        GameManager.Instance.ONHowToEnter += () => gameObject.SetActive(true);
    }
}
