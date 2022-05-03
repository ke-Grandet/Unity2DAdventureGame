using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{

    [Range(0, 10)] public int jumpVelocity = 6;  // ��Ծ����
    public LayerMask mask = 89;  // ������ײ����ͼ�㼯��
    public float boxHeight;  // �ײ��������ĸ߶�

    private Vector2 playerSize;  // ��Ҵ�С
    private Vector2 boxSize;  // �ײ���������С

    private bool jumpRequest = false;  // ����Ƿ�����Ծ
    private bool grounded = false;  // ����Ƿ��ŵ�

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;  // ��ȡ��Ҵ�С
        boxSize = new Vector2(playerSize.x * 0.8f, boxHeight);  // �ײ�����������ҳ���Ϊ��ҵ�0.8��
    }

    // Update is called once per frame
    void Update()
    {
        // ��������Ծ�ˣ����������ŵ�״̬����ô������Ծ
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpRequest = true;
        }
    }

    void FixedUpdate()
    {
        // ��Ծʱ�Ĵ���
        if (jumpRequest)
        {
            // ��һ�����ϵ���
            _rigidbody2D.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            // ������Ծ��ʶ
            jumpRequest = false;
            // �ŵر�ʶ��Ϊ��
            grounded = false;
        }
        // û����Ծʱ�Ĵ���
        else
        {
            // ����ײ�������򣬴��������������һ����Ҵ�С��λ��
            Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize * 0.5f);

            // ���������ײ
            if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
            {
                // �ŵر�ʶ��Ϊ��
                grounded = true;
            }
            // ���������ײ
            else
            {
                // �ŵر�ʶ��Ϊ��
                grounded = false;
            }
        }
    }

    // �����ײ������Ա��ڹ۲�
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
