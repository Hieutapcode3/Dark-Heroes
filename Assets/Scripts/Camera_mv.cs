using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_mv : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]  public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x -1,player.position.y + 2,transform.position.z);
    }
}
