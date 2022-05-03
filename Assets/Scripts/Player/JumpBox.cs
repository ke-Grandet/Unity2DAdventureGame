using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{

    [Range(0, 10)] public int jumpVelocity = 6;  // 跳跃速率
    public LayerMask mask = 89;  // 用于碰撞检测的图层集合
    public float boxHeight;  // 底部检测区域的高度

    private Vector2 playerSize;  // 玩家大小
    private Vector2 boxSize;  // 底部检测区域大小

    private bool jumpRequest = false;  // 玩家是否在跳跃
    private bool grounded = false;  // 玩家是否着地

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;  // 获取玩家大小
        boxSize = new Vector2(playerSize.x * 0.8f, boxHeight);  // 底部检测区域左右长度为玩家的0.8倍
    }

    // Update is called once per frame
    void Update()
    {
        // 如果玩家跳跃了，且正处于着地状态，那么处理跳跃
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpRequest = true;
        }
    }

    void FixedUpdate()
    {
        // 跳跃时的处理
        if (jumpRequest)
        {
            // 加一个向上的力
            _rigidbody2D.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            // 重置跳跃标识
            jumpRequest = false;
            // 着地标识置为假
            grounded = false;
        }
        // 没在跳跃时的处理
        else
        {
            // 定义底部检测区域，处于玩家中心下移一半玩家大小的位置
            Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize * 0.5f);

            // 与地面有碰撞
            if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
            {
                // 着地标识置为真
                grounded = true;
            }
            // 与地面无碰撞
            else
            {
                // 着地标识置为假
                grounded = false;
            }
        }
    }

    // 画出底部检测框，以便于观察
    private void OnDrawGizmos()
    {
        if (grounded)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize * 0.5f);
        //Gizmos.DrawWireCube(boxCenter, boxSize);
    }

}
