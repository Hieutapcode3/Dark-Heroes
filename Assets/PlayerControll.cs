using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void Movement()
    {
        float horizontallInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontallInput * speed, rb.velocity.y);
        if (horizontallInput > 0)
            transform.localScale = Vector2.one * 4.5f;
        else if (horizontallInput < 0)
            transform.localScale = new Vector2(-1,1) *4.5f;
        if (Input.GetKey(KeyCode.Space))
            rb.velocity = new Vector2(rb.velocity.x, speed);

        anim.SetBool("run",horizontallInput!=0);
    }
}
