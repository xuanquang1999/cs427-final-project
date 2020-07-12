using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid;
    public float moveSpeed = 5f;
    float currentMove;
    bool isMove = false;

    public float jumpSpeed = 12f;
    float jumpMove;

    float lookDirection = 1;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //Khởi tạo lấy các thành phần trước
        rigid = GetComponent<Rigidbody2D>();

        //Gán giá trị về tốc độ đi và nhảy ban đầu là 0
        currentMove = 0;
        jumpMove = 0;

        anim = GetComponent<Animator>();
    }

    // Lưu ý: xử lý input thì ở update, xử lý vật lý ở fixed update
    void Update()
    {
        //Gán tốc độ nhảy chứ ko xử trực tiếp ở update, xử lý vật lý ở fixed update
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpMove = jumpSpeed;
        }

        //Gán tốc độ đi chứ ko xử trực tiếp ở update, xử lý vật lý ở fixed update
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Đi trái thì hướng phải ngược lại, do đó có giá trị âm
            currentMove = -moveSpeed;

            //Gọi animation walking chạy 1 lần
            anim.SetFloat("velX", -0.5f);

            lookDirection = -1;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //Đi phải thì hướng cùng chiều, do đó có giá trị dương
                currentMove = moveSpeed;

                //Gọi animation walking chạy 1 lần
                anim.SetFloat("velX", 0.5f);

                lookDirection = 1;
            }
            else
            {
                //Nếu không di chuyển (không nhấn nút nào), gán giá trị tốc độ = 0 (đứng yên)
                currentMove = 0;
                anim.SetFloat("velX", 0);
            }
        }

        anim.SetFloat("lookDirection", lookDirection);
    }

    //Xử lý vật lý ở fixed update
    private void FixedUpdate()
    {
        //Làm đóng băng góc xoay để tránh bị tác động của lực vật lý làm nhân vật lăn lộn
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        //2 cách để di chuyển nhận vật: 
        // Cách 1: thay đổi vận tốc đều, giá trị vận tốc đã cập nhật trên Update()
        rigid.velocity = new Vector2(currentMove, rigid.velocity.y + jumpMove);

        //Cách 2: Tác động 1 lực theo chiều x (di chuyển trái phải) và theo chiều y (nhảy)
        //Tác động lực theo luật Newton, do đó lực sẽ giảm dần và nhân vật có thể bị trượt
        //Tăng khổi lượng (mass), tăng linear drag (lực cản theo phương ngang) để giảm độ trượt
        //rigid.AddForce(new Vector2(currentMove, jumpMove));

        //Sau khi xử lý vật lý xong, gán lại tốc độ nhảy bằng 0 (để cho rớt xuống theo trọng lực)
        jumpMove = 0;
    }
}
