using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float speed = 5;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private float x = 0;
    private float y = 0;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        
        if (_rigidbody2D.bodyType != RigidbodyType2D.Static)
        {
            if (x > 0)
            {
                _rigidbody2D.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                _animator.SetBool("run", true);
            }
            if (x < 0)
            {
                _rigidbody2D.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                _animator.SetBool("run", true);
            }

            if (x < 0.001f && x > -0.001f)
            {
                _animator.SetBool("run", false);
            }
        }

    }

    private void FixedUpdate()
    {
        if (_rigidbody2D.bodyType != RigidbodyType2D.Static)
        {
            Run();  // �����ƶ�
        }
    }

    // ��ײ�¼�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Saw"))
        {
            Destroy(gameObject);
            GameController.Instance.ShowGameOverPanel();
        }
    }

    private void Run()
    {
        // ͨ���޸ĸ����ٶ�ʵ�������ƶ���ע����ײ��Ҫʹ����Ħ�����Ĳ�����
        Vector2 velocity = _rigidbody2D.velocity;
        velocity.x = x * speed;
        _rigidbody2D.velocity = velocity;
        // ����ֱ���޸ĸ����λ��
        //Vector2 movement = new Vector2(x, 0);
        //_rigidbody2D.position += speed * Time.deltaTime * movement;
    }

}
