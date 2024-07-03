using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTxt;
    public GameObject healTxtprefab;

    public Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    public void ChracterTookDamage(GameObject character , int damageReceive)
    {
        Vector3 spawPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        
        TMP_Text tmpTxt = Instantiate(damageTxt,spawPosition,Quaternion.identity,gameCanvas.transform).GetComponent<TMP_Text>();

        tmpTxt.text = damageReceive.ToString();
    }
    public void ChracterHealth(GameObject character, int HealthREstored)
    {
        Vector3 spawPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpTxt = Instantiate(damageTxt, spawPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpTxt.text = HealthREstored.ToString();
    }
}
