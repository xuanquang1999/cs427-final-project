using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButtonHandler : MonoBehaviour
{
    public GameObject panel;

    public void OnClickHandler()
    {
        panel.SetActive(false);
    }
}
