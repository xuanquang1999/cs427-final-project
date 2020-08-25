using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CharacterController controller = collider.GetComponent<CharacterController>();
            controller.GetMushroom();
            Destroy(gameObject);
        }
    }
}
