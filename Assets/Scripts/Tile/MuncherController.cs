using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuncherController : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CharacterController controller = collider.GetComponent<CharacterController>();
            controller.Hurt();
        }
    }
}
