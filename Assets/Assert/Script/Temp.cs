using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("������");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("������ײ");
    }
}
