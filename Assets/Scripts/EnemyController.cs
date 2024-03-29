using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float Speed = 1.5f;
    [SerializeField] private float CurrentTime ;
    [SerializeField] private float RangeTime = 2f ;
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
    public void setSpeed(float speed)
    {
        this.Speed = speed;
    }
    public float getSpeed()
    {
        return Speed;
    }
    protected void Movement()
    {
        anim.SetInteger("State", 2);
        Vector3 target = transforms[currentposition].position;
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);

        // Kiểm tra nếu đã đến điểm đích
        if (transform.position == transforms[currentposition].position)
        {
            // Dừng animation và kiểm tra thời gian
            anim.SetInteger("State", 0);
            setSpeed(0);
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= RangeTime)
            {
                anim.SetInteger("State", 2);
                setSpeed(1.5f);
                CurrentTime = 0f;
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
}


