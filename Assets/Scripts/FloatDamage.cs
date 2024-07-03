using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public float destroyTime = 3f;
    void Start()
    {
        Destroy(gameObject,destroyTime);
    }


}
