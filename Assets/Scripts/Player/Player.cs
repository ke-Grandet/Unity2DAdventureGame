using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移动速率")]
    public float speed = 5f;
    [Header("跳跃力")]
    [Range(0, 10)]
    public int jumpForce = 5;
    [Header("跳跃段数")]
    [Range(1, 2)]
    public int jumpTimes = 2;  // 段数为2即二段跳
    [Header("可触地的图层")]
    public LayerMask groundLayer;  // 地面、平台等

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider;
    private SpriteRenderer _spriteRenderer;

    private float horizontal = 0;  // 横轴输入
    private bool jumpRequest = false;  // 跳跃键输入
    private int jumpRemainingTimes;  //  剩余跳跃段数
    private bool isOnGround = true;  // 是否触地

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        jumpRemainingTimes = jumpTimes;

    }

    // Update is called once per frame
    void Update()
    {
        if (!_rigidbody.simulated)
        {
            return;
        }

        //---- 获取横轴输入
        horizontal = Input.GetAxis("Horizontal");
        // 如果横轴输入为0
        if (Mathf.Approximately(horizontal, 0f))
        {
            // 切换为站立动画
            _animator.SetBool("Move", false);
        }
        // 如果横轴输入不为0
        else
        {
            // 切换为跑动动画
            _animator.SetBool("Move", true);
            // 动画朝右
            if (horizontal > 0f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            // 动画朝左
            else
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }


        //---- 获取纵轴速度
        float velocityY = _rigidbody.velocity.y;
        // 切换起跳动画/下落动画
        _animator.SetFloat("Velocity Y", Mathf.Approximately(velocityY, 0f) ? 0f : velocityY);


        //---- 触地检测，画一条从左脚到右脚的线段来与地板进行碰撞检测
        Vector2 rayOrigin = new(_rigidbody.position.x - _capsuleCollider.bounds.size.x / 2 + 0.1f,
            _rigidbody.position.y - _spriteRenderer.bounds.size.y / 2);
        RaycastHit2D raycastHit = Physics2D.Raycast(
            rayOrigin, Vector2.right, _capsuleCollider.bounds.size.x - 0.2f, groundLayer);
        // 更新触地变量
        isOnGround = raycastHit.collider != null;
        if (isOnGround)
        {
            jumpRemainingTimes = jumpTimes;
        }
        // 获取跳跃键输入
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
            if (!isOnGround && jumpRemainingTimes > 0)
            {
                // 切换二段跳动画
                _animator.SetTrigger("Double Jump");
            }
        }
        // 画线便于观察
        Debug.DrawRay(rayOrigin, Vector2.right * _capsuleCollider.bounds.size.x, raycastHit.collider != null ? Color.green : Color.red);

    }

    private void FixedUpdate()
    {
        if (!_rigidbody.simulated)
        {
            return;
        }

        // 左右移动
        Move();

        // 跳跃
        if (jumpRequest)
        {
            // 如果未触地，且未跳跃过
            if (!isOnGround && jumpRemainingTimes == jumpTimes)
            {
                // 先消耗一次跳跃段数
                jumpRemainingTimes--;
            }
            if (jumpRemainingTimes > 0)
            {
                // 清零垂直速度
                _rigidbody.velocity = new(_rigidbody.velocity.x, 0);
                // 加一个向上的力
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                // 剩余跳跃段数减一
                jumpRemainingTimes--;
            }
            // 清除跳跃键输入
            jumpRequest = false;
        }
    }

    // 碰撞事件
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Saw"))
        {
            Hit();
        }
    }

    private void Move()
    {
        // 通过修改刚体速度实现左右移动，注意刚体和碰撞箱要使用无摩擦力的材质球
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = horizontal * speed;
        _rigidbody.velocity = velocity;
    }

    // 受击
    public void Hit()
    {
        // 取消刚体的物理模拟
        _rigidbody.simulated = false;
        // 播放战败动画，动画脚本中调用战败方法
        _animator.SetTrigger("Hit");
    }

    // 战败
    public void Defeated()
    {
        Destroy(gameObject);
        GameController.Instance.ShowGameOverPanel();
    }

}
