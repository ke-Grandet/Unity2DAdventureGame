using Enum = System.Enum;
using Array = System.Array;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    [Header("�ƶ�����")]
    [Range(1, 10)]
    public int speed = 4;
    [Header("�������ֵ")]
    public int maxHealth = 10;
    [Header("ս��Ʒ")]
    public GameObject booty;
    [Header("����ƶ�ʱ��")]
    public float minMoveTime = 1f;
    [Header("��ƶ�ʱ��")]
    public float maxMoveTime = 3f;
    [Header("ǽ��ͼ��")]
    public LayerMask boundLayer;

    private int direction = -1;  // �ƶ�����
    private float moveTime = 2f;  // �ƶ�ʱ��
    private int health;  // ��ǰ����ֵ

    private bool isMoving = true;  // �Ƿ������ƶ�

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private BoxCollider2D _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            return;
        }

        // ����ǽ�ں��ͷ
        Vector2 rayOrigin = new(_rigidbody.position.x + direction * _boxCollider.bounds.size.x / 2,
            _rigidbody.position.y);
        RaycastHit2D raycastHit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, 0.1f, boundLayer);
        if (raycastHit.collider != null)
        {
            direction = -direction;
            _animator.SetFloat("Direction", direction);  // �����ƶ�����
        }
        
        if (isMoving)
        {
            moveTime -= Time.deltaTime;
            if (moveTime <= 0)
            {
                RandomAct();
            }
        }

    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            return;
        }

        if (isMoving)
        {
            _rigidbody.velocity = new(direction * speed, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new(0f, _rigidbody.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (collision.GetContact(0).point.y >= _boxCollider.bounds.max.y - 1f)
            {
                Hit(1);
                player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
            }
            else
            {
                player.Hit();
            }
        }
    }

    // ����
    public void Attack()
    {
        Debug.Log("boss���ڶ������Թ���");
    }

    // �ܻ�
    public void Hit(int damage)
    {
        health -= damage;
        float healthPercent = Mathf.Clamp01(health / (float)maxHealth);
        HealthBar.Instance.ChangeValue(healthPercent);
        if (health <= 0)
        {
            _rigidbody.simulated = false;  // ���ø���
            _animator.SetInteger("Health", health);  // ������������
            gameObject.GetComponentInChildren<ParticleSystem>().Play();  // ����������Ч
            GameController.Instance.GainScore(5000);
            Invoke(nameof(Booty), 2f);
            Destroy(gameObject, 2f);
        }
    }

    // ���ѡ��һ������
    public void RandomAct()
    {
        RandomAct(Random.Range(0, 2));
    }

    public void RandomAct(int act)
    {
        switch (act)
        {
            // �ƶ�
            case 0:
                isMoving = true;
                direction = -direction;  // ��ת�ƶ�����
                moveTime = Random.Range(minMoveTime, maxMoveTime);  // ˢ���ƶ�ʱ��
                _animator.SetFloat("Direction", direction);  // �����ƶ�����
                break;
            // ����
            case 1:
                isMoving = false;
                _animator.SetTrigger("Attack");  // ���Ź�������
                break;
            // ����
            default:
                Debug.Log("����������ֵ");
                break;
        }
    }

    // ����ս��Ʒ
    public void Booty()
    {
        GameObject gameObject = Instantiate(booty, _rigidbody.position, Quaternion.identity);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

}
