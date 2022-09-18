using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHouse : MonoBehaviour
{
    public House hS;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseDown()
    {
        if (hS.Collect_Money == true)
        {
            hS.Collect_Money_Icon.SetActive(false);
            hS.Collect_Money = false;
            hS.gMS.money += hS.Money_In_Neighborhood;
            hS.Money_In_Neighborhood = 0f;

            //collectPower_Sfx.Play();


        }
        else
        {
            hS.Open_Neighbourhood_UI();
        }

    }
}