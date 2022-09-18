using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockWindmill : MonoBehaviour
{
    public GameManagerScript gMS;

    public float cost = 0f;
    public TextMeshProUGUI costDisplay;

    public GameObject windmill_Unlock_UI;
    public GameObject windmill_Buy_UI;

    [SerializeField] private AudioSource unlockWindmill_sfx;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        costDisplay.text = cost.ToString() + "$";

        if (cost > gMS.money)
        {
            costDisplay.color = Color.red;
        }
        else
        {
            costDisplay.color = Color.green;
        }
    }


    public void UnlockWindmill_()
    {
        if(gMS.money >= cost)
        {
            unlockWindmill_sfx.Play();
            windmill_Buy_UI.SetActive(true);
            windmill_Unlock_UI.SetActive(false);
        }
        else
        {
            gMS.YouAreBrokeSFX();
        }
    }
}
