using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;

    private BoxCollider2D boxCollider;
    private Animator animator;
    private Rigidbody2D rigid;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckPlayerCollision();
    }

    private void CheckPlayerCollision()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.up, 0.1f, playerLayerMask);
        if (raycastHit.collider != null)
        {
            float playerBottom = raycastHit.collider.bounds.min.y;
            float spriteTop = boxCollider.bounds.max.y;
            //Debug.Log(String.Format("playerBottom: {0}; spriteTop: {1}", playerBottom, spriteTop));
            if (playerBottom > spriteTop - 0.2f)
            {
                float playerVelocityY = raycastHit.collider.attachedRigidbody.velocity.y;
                float enemyVelocityY = rigid.velocity.y;
                if (playerVelocityY < enemyVelocityY)
                {
                    raycastHit.collider.GetComponent<CharacterController>().BounceUpFromEnemy();
                    StartCoroutine(Kill());
                }
            }
            else
            {
                raycastHit.collider.GetComponent<CharacterController>().Hurt();
            }
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Collider2D collider = collision.collider;
    //    if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        CharacterController controller = collider.GetComponent<CharacterController>();
    //        controller.Hurt();
    //    }
    //}

    private IEnumerator Kill()
    {
        animator.SetTrigger("Die");
        boxCollider.enabled = false;        
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSecondsRealtime(1f);

        Destroy(gameObject);
    }
}
