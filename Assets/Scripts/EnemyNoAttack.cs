using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNoAttack : MonoBehaviour
{
    [SerializeField] private float Speed = 2f;
    [SerializeField] private float CurrentTime;
    [SerializeField] private float RangeTime = 3f;
    private int currentposition = 0;
    protected Animator anim;
    [SerializeField] protected Transform[] transformsPos;
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    protected void Movement()
    {
        anim.SetBool("Move", true);
        Vector3 target = transformsPos[currentposition].position;
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (target.x < transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (target.x > transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (transform.position == transformsPos[currentposition].position)
        {
            anim.SetBool("Move", false);
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= RangeTime)
            {
                anim.SetBool("Move", true);
                CurrentTime = 0f;
                currentposition++;
                if (currentposition >= transformsPos.Length)
                {
                    currentposition = 0;
                }

            }

        }
    }
}
