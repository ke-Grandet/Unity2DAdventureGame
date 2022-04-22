using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{

    public Canvas canvas;

    private BoxCollider2D _boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(true);
            _boxCollider2D.enabled = false;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.GetComponent<Animator>().enabled = false;
        }
    }

}
