using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider);

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CharacterController controller = collider.GetComponent<CharacterController>();
            StartCoroutine(controller.Kill());
        }
    }
}
