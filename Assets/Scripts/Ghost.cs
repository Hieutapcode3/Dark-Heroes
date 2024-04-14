using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float  ghostDelay;
    private float           ghostDelaySecond;
    public GameObject       ghost;
    public bool            makeGhost;

    void Start()
    {
        ghostDelaySecond = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySecond > 0)
            {
                ghostDelaySecond -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySecond = ghostDelay;
                Destroy(currentGhost,1f);

            }
        }
        
    }
}
