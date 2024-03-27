using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack :  EnemyController
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator MoveControll()
    {
        while (true)
        {
            if(transform.position == transforms[1].position)
            {
                anim.SetInteger("State", 0);
                yield return new WaitForSeconds(2f);
            }
        }
        
    }
}
