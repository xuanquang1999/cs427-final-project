using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameConstants;

public class CharacterController : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float movementSmoothing;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject UIManager;

    //private bool isGrounded = false;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Vector3 velocity;
    private bool isFacingRight;

    private int life;
    private int coinCount;
    private PlayerStatus playerStatus;

    [SerializeField] private float timeInvincible;
    private bool isInvincible;
    private float invincibleTimer;

    private bool currentJump;

    public bool allowInput;

    //[SerializeField] private int flashRate = 4; 
    //int nextInvincibleFlash = 0;
    //int;

    //[Header("Events")]
    //[Space]
    //public UnityEvent OnHurtEvent;

    //[System.Serializable]
    //public class BoolEvent : UnityEvent<bool> { }
    //public BoolEvent OnCrouchEvent;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        velocity = Vector3.zero;
        isFacingRight = true;

        life = 5;
        coinCount = 0;
        playerStatus = PlayerStatus.STATUS_SMALL;

        isInvincible = false;
        invincibleTimer = 0;
        allowInput = true;
    }

    private void Update()
    {
        if (!allowInput)
            return;

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            { 
                isInvincible = false;
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }

    }

    //private void OnCollisionEnter2D(Collision2D collider)
    //{
    //    Debug.Log("Enter collision");
    //    if (rigid.velocity.y <= 0)
    //        isGrounded = true;
    //}

    //private void OnCollisionExit2D()
    //{
    //    isGrounded = false;
    //}

    public void Move(float move, bool jump, bool stopJump)
    {
        //Làm đóng băng góc xoay để tránh bị tác động của lực vật lý làm nhân vật lăn lộn
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rigid.velocity.y);
        // And then smoothing it out and applying it to the character
        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (jump)
        {
            // If the player should jump...
            if (IsGrounded()) {
                // Add a vertical force to the player.
                //rigid.velocity = new Vector2(rigid.velocity.x, jumpVelocity);
                rigid.AddForce(new Vector2(0, 800f));
            }

            //Debug.Log("stopJump: " + stopJump);
        }

        if (stopJump) 
        {            
            if (rigid.velocity.y > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
            }
        }

        if ((isFacingRight && move < 0) || (!isFacingRight && move > 0))
        {
            Flip();
        }

        if (jump)
            currentJump = true;
        if (stopJump)
            currentJump = false;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.05f, platformLayerMask);        
        return raycastHit.collider != null;
    }

    private void Flip()
    {
        Vector3 newScale = rigid.transform.localScale;
        newScale.x *= -1;
        rigid.transform.localScale = newScale;

        isFacingRight = !isFacingRight;
    }

    public void ChangeCoin(int deltaCoin)
    {
        coinCount = coinCount + deltaCoin;
        if (coinCount >= 100)
        {
            coinCount -= 100;
            life += 1;
        }            
    }

    public void ChangeStatus(PlayerStatus newStatus)
    {
        playerStatus = newStatus;
        int tmp = (int)newStatus;
        Debug.Log(newStatus + ": " + tmp);
        animator.SetFloat("Status", tmp);
    }

    public void GetMushroom()
    {
        if (playerStatus == PlayerStatus.STATUS_SMALL)
        {
            ChangeStatus(PlayerStatus.STATUS_BIG);
        }
        else
        {
            // TODO: Add preserved mushroom
        }
    }

    public void Hurt()
    {
        if (isInvincible)
            return;

        if (playerStatus == PlayerStatus.STATUS_BIG)
        {

            isInvincible = true;
            invincibleTimer = timeInvincible;
            ChangeStatus(PlayerStatus.STATUS_SMALL);
        }
        else if (playerStatus == PlayerStatus.STATUS_SMALL)
        {
            StartCoroutine(Kill());
        }
    }

    public void BounceUpFromEnemy()
    {
        //Debug.Log(currentJump);
        if (currentJump)
            rigid.velocity = new Vector2(rigid.velocity.x, 18f);
        else
            rigid.velocity = new Vector2(rigid.velocity.x, 10f);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public IEnumerator Kill()
    {
        allowInput = false;

        //PauseGame();

        animator.SetTrigger("DieIdle");
        boxCollider.enabled = false;
        rigid.isKinematic = true;
        rigid.velocity = new Vector2(0, 0);

        yield return new WaitForSecondsRealtime(0.5f);

        animator.SetTrigger("Die");
        rigid.isKinematic = false;
        rigid.velocity = new Vector2(0, jumpVelocity);

        yield return new WaitForSecondsRealtime(2.5f);

        UIManager.GetComponent<GameUIManager>().PlayerDie();
        Destroy(gameObject);
    }

    public void CompleteLevel()
    {
        allowInput = false;
        boxCollider.enabled = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Victory");

        UIManager.GetComponent<GameUIManager>().CompleteLevel();
    }
}
