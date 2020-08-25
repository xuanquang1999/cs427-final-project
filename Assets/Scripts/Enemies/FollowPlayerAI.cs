using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerAI : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private LayerMask enemiesLayerMask;
    [SerializeField] private bool isFacingRight;
    [SerializeField] private float speed;

    private GameObject player;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private Rigidbody2D rigid;
    private Vector3 velocity;

    private float updateDirectionTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        velocity = Vector3.zero;
        
        if (!isFacingRight)
        {
            Vector3 newScale = rigid.transform.localScale;
            newScale.x *= -1;
            rigid.transform.localScale = newScale;
        }
    }

    void FixedUpdate()
    {
        updateDirectionTimer -= Time.deltaTime;
        if (updateDirectionTimer < 0)
        {
            if (isFacingRight ^ (transform.position.x < player.transform.position.x))
                Flip();
            updateDirectionTimer = 0.5f;
        }

        int direction = isFacingRight ? 1 : -1;
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(direction * speed, rigid.velocity.y);
        // And then smoothing it out and applying it to the character
        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (TouchWall(direction))
        {
            Flip();
        }
    }

    private bool TouchWall(int direction)
    {
        Vector3 boxSizeEpsilon = new Vector3(boxCollider.bounds.size.x - 0.02f, boxCollider.bounds.size.y - 0.02f);
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(boxCollider.bounds.center, boxSizeEpsilon, 0f, Vector2.right * direction, 0.05f, platformLayerMask | enemiesLayerMask);
        foreach(RaycastHit2D hit in raycastHits)
        {
            if (hit.collider != boxCollider)
                return true;
        }
        return false;
    }

    private void Flip()
    {
        Vector3 newScale = rigid.transform.localScale;
        newScale.x *= -1;
        rigid.transform.localScale = newScale;

        isFacingRight = !isFacingRight;
    }    
}
