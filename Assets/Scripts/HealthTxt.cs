using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthTxt : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0,75,0);
    public float timeDestroy = 1f;
    private float timeElapsed = 0f;

    RectTransform txtTransform;
    Text txtMeshpro;
    private Color startColor;
    void Awake()
    {
        txtTransform = GetComponent<RectTransform>();
        txtMeshpro = GetComponent<Text>();
        startColor = txtMeshpro.color;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        txtTransform.position += moveSpeed * Time.deltaTime;
        if (timeElapsed < timeDestroy)
        {
            float fadeAlpha = startColor.a * (1- (timeElapsed/timeDestroy));
            txtMeshpro.color = new Color(startColor.r, startColor.g, startColor.b,fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
