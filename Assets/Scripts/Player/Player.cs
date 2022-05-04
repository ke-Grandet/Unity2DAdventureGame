using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float speed = 5f;
    [Header("��Ծ��")]
    [Range(0, 10)]
    public int jumpForce = 5;
    [Header("��Ծ����")]
    [Range(1, 2)]
    public int jumpTimes = 2;  // ����Ϊ2��������
    [Header("�ɴ��ص�ͼ��")]
    public LayerMask groundLayer;  // ���桢ƽ̨��

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider;
    private SpriteRenderer _spriteRenderer;

    private float horizontal = 0;  // ��������
    private bool jumpRequest = false;  // ��Ծ������
    private int jumpRemainingTimes;  //  ʣ����Ծ����
    private bool isOnGround = true;  // �Ƿ񴥵�

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

        //---- ��ȡ��������
        horizontal = Input.GetAxis("Horizontal");
        // �����������Ϊ0
        if (Mathf.Approximately(horizontal, 0f))
        {
            // �л�Ϊվ������
            _animator.SetBool("Move", false);
        }
        // ����������벻Ϊ0
        else
        {
            // �л�Ϊ�ܶ�����
            _animator.SetBool("Move", true);
            // ��������
            if (horizontal > 0f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            // ��������
            else
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }


        //---- ��ȡ�����ٶ�
        float velocityY = _rigidbody.velocity.y;
        // �л���������/���䶯��
        _animator.SetFloat("Velocity Y", Mathf.Approximately(velocityY, 0f) ? 0f : velocityY);


        //---- ���ؼ�⣬��һ������ŵ��ҽŵ��߶�����ذ������ײ���
        Vector2 rayOrigin = new(_rigidbody.position.x - _capsuleCollider.bounds.size.x / 2 + 0.1f,
            _rigidbody.position.y - _spriteRenderer.bounds.size.y / 2);
        RaycastHit2D raycastHit = Physics2D.Raycast(
            rayOrigin, Vector2.right, _capsuleCollider.bounds.size.x - 0.2f, groundLayer);
        // ���´��ر���
        isOnGround = raycastHit.collider != null;
        if (isOnGround)
        {
            jumpRemainingTimes = jumpTimes;
        }
        // ��ȡ��Ծ������
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
            if (!isOnGround && jumpRemainingTimes > 0)
            {
                // �л�����������
                _animator.SetTrigger("Double Jump");
            }
        }
        // ���߱��ڹ۲�
        Debug.DrawRay(rayOrigin, Vector2.right * _capsuleCollider.bounds.size.x, raycastHit.collider != null ? Color.green : Color.red);

    }

    private void FixedUpdate()
    {
        if (!_rigidbody.simulated)
        {
            return;
        }

        // �����ƶ�
        Move();

        // ��Ծ
        if (jumpRequest)
        {
            // ���δ���أ���δ��Ծ��
            if (!isOnGround && jumpRemainingTimes == jumpTimes)
            {
                // ������һ����Ծ����
                jumpRemainingTimes--;
            }
            if (jumpRemainingTimes > 0)
            {
                // ���㴹ֱ�ٶ�
                _rigidbody.velocity = new(_rigidbody.velocity.x, 0);
                // ��һ�����ϵ���
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                // ʣ����Ծ������һ
                jumpRemainingTimes--;
            }
            // �����Ծ������
            jumpRequest = false;
        }
    }

    // ��ײ�¼�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Saw"))
        {
            Hit();
        }
    }

    private void Move()
    {
        // ͨ���޸ĸ����ٶ�ʵ�������ƶ���ע��������ײ��Ҫʹ����Ħ�����Ĳ�����
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = horizontal * speed;
        _rigidbody.velocity = velocity;
    }

    // �ܻ�
    public void Hit()
    {
        // ȡ�����������ģ��
        _rigidbody.simulated = false;
        // ������������
        _animator.SetTrigger("Hit");
    }

}
