using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigid;
    CharacterController controller;
    Animator anim;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    float move = 0f;
    bool jump = false;
    bool stopJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Lưu ý: xử lý input thì ở update, xử lý vật lý ở fixed update
    void Update()
    {
        if (!controller.allowInput)
            return;

        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Run"))
        {
            move *= runSpeed;
        } else
        {
            move *= walkSpeed;
        }

        anim.SetFloat("SpeedX", Mathf.Abs(move) / 20.0f);
        anim.SetFloat("VelocityY", rigid.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            stopJump = true;
        }
    }

    //Xử lý vật lý ở fixed update
    private void FixedUpdate()
    {
        if (!controller.allowInput)
            return;

        controller.Move(move * Time.fixedDeltaTime, jump, stopJump);
        jump = false;
        stopJump = false;
    }
}
