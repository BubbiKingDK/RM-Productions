using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cost : MonoBehaviour
{
    public GameManagerScript gMS;

    public float cost = 0f;
    public TextMeshProUGUI costDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        costDisplay.text = cost.ToString()+"$";

        if(cost> gMS.money)
        {
            costDisplay.color = Color.red;
        }
        else
        {
            costDisplay.color = Color.white;
        }
    }
}
