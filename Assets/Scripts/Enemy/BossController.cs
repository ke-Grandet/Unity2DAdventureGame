using Enum = System.Enum;
using Array = System.Array;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    [Header("移动速率")]
    [Range(1, 10)]
    public int speed = 4;
    [Header("最大生命值")]
    public int maxHealth = 10;
    [Header("战利品")]
    public GameObject booty;
    [Header("最短移动时间")]
    public float minMoveTime = 1f;
    [Header("最长移动时间")]
    public float maxMoveTime = 3f;
    [Header("墙壁图层")]
    public LayerMask boundLayer;

    private int direction = -1;  // 移动方向
    private float moveTime = 2f;  // 移动时间
    private int health;  // 当前生命值

    private bool isMoving = true;  // 是否正在移动

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

        // 碰到墙壁后掉头
        Vector2 rayOrigin = new(_rigidbody.position.x + direction * _boxCollider.bounds.size.x / 2,
            _rigidbody.position.y);
        RaycastHit2D raycastHit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, 0.1f, boundLayer);
        if (raycastHit.collider != null)
        {
            direction = -direction;
            _animator.SetFloat("Direction", direction);  // 播放移动动画
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

    // 攻击
    public void Attack()
    {
        Debug.Log("boss正在对你语言攻击");
    }

    // 受击
    public void Hit(int damage)
    {
        health -= damage;
        float healthPercent = Mathf.Clamp01(health / (float)maxHealth);
        HealthBar.Instance.ChangeValue(healthPercent);
        if (health <= 0)
        {
            _rigidbody.simulated = false;  // 禁用刚体
            _animator.SetInteger("Health", health);  // 播放死亡动画
            gameObject.GetComponentInChildren<ParticleSystem>().Play();  // 播放粒子特效
            GameController.Instance.GainScore(5000);
            Invoke(nameof(Booty), 2f);
            Destroy(gameObject, 2f);
        }
    }

    // 随机选择一个动作
    public void RandomAct()
    {
        RandomAct(Random.Range(0, 2));
    }

    public void RandomAct(int act)
    {
        switch (act)
        {
            // 移动
            case 0:
                isMoving = true;
                direction = -direction;  // 调转移动方向
                moveTime = Random.Range(minMoveTime, maxMoveTime);  // 刷新移动时间
                _animator.SetFloat("Direction", direction);  // 播放移动动画
                break;
            // 攻击
            case 1:
                isMoving = false;
                _animator.SetTrigger("Attack");  // 播放攻击动画
                break;
            // 其它
            default:
                Debug.Log("出现意外数值");
                break;
        }
    }

    // 产生战利品
    public void Booty()
    {
        GameObject gameObject = Instantiate(booty, _rigidbody.position, Quaternion.identity);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }

}
