using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,ITakeDamage
{
    [Header("Movement")]
    [SerializeField] private float          Speed = 1.5f;
    [SerializeField] private float          CurrentTime ;
    [SerializeField] private float          RangeTime = 3f ;
    private int                             currentposition = 0;

    [Header("Attack")]
    [SerializeField] private float          attackCooldown;
    [SerializeField] private float          range;
    [SerializeField] private float          colliderDistance;
    [SerializeField] private int            damage;
    private float                           cooldownTime = Mathf.Infinity;

    [Header("Health")]
    [SerializeField] private int health = 30;

    [Header("Other")]
    [SerializeField] private BoxCollider2D  boxCollider;
    [SerializeField] private LayerMask      playerLayer;
    protected Rigidbody2D                   rb;
    protected Animator                      anim;
    [SerializeField] protected Transform[]  transformsPos;
    [SerializeField] private PlayerControll player;
    public GameObject                       DamageTxt;
    public Vector3 moveSpeed = new Vector3(0,25,0);
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        cooldownTime += Time.deltaTime;
        if (PlayerInSight() && player.health > 0)
        {
            if (cooldownTime >= attackCooldown)
            {
                anim.SetTrigger("Attack");
                anim.SetBool("moving", false);
                cooldownTime = 0;
            }
        }else
           Movement();
    }
    protected void Movement()
    {
        anim.SetBool("moving", true);
        Vector3 target = transformsPos[currentposition].position;
        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (transform.position == transformsPos[currentposition].position)
        {
            anim.SetBool("moving", false);
            CurrentTime += Time.deltaTime;
            if (CurrentTime >= RangeTime)
            {
                anim.SetBool("moving", true);
                CurrentTime = 0f;
                currentposition++;
                if (currentposition >= transformsPos.Length)
                {
                    currentposition = 0;
                }
                if (currentposition == 1)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
        
        }
    }

    public void takeDamage(int damage)
    {
        anim.SetTrigger("hit");
        health -= damage;
        if (DamageTxt)
        {
            ShowDamageTxt(damage);
        }
        if (health <= 0)
            StartCoroutine(Dead());
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x  * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y ,boxCollider.bounds.size.z),
            0,Vector2.left,0, playerLayer);
        return hit.collider != null;
    }
    private IEnumerator Dead()
    {
        anim.SetTrigger("Dead");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    private void DamagePlayer()
    {
        if(PlayerInSight())
        {
            player.takeDamage(damage);
        }
    }
    private void ShowDamageTxt(int x)
    {
        GameObject parentObject = new GameObject("DamageTextParent");
        parentObject.transform.position = transform.position;
        parentObject.transform.rotation = Quaternion.identity;
        parentObject.transform.parent = transform;
        var go = Instantiate(DamageTxt, parentObject.transform.position, Quaternion.identity, parentObject.transform);
        go.GetComponent<TextMesh>().text = x.ToString();
        go.transform.position += moveSpeed * Time.deltaTime;
        go.transform.localScale = new Vector3(Mathf.Abs(go.transform.localScale.x), go.transform.localScale.y, go.transform.localScale.z);

    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        //    new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}


