using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float Speed = 1.5f;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;

    [SerializeField] protected Transform[] transforms;
    private int currentposition = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        
    }
     protected void Movement()
    {
        Vector3 target = transforms[currentposition].position;
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (transform.position == transforms[currentposition].position)
        {
            currentposition++;
            if (currentposition >= transforms.Length)
            {
                currentposition = 0;
            }
            if (currentposition == 1)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
