using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class House : MonoBehaviour
{
    [Header("Test")]
    public bool run = false;

    public bool SellPowerToNeighborhood = false;

    [Header("Production")]
    public float house_Count = 0f;
    public float house_Energy_Usage = 0f;
    public float house_Payment = 0f;

    public float total_neighborhood_power_usage;
    public float total_neighborhood_payment;


    public bool Ready_to_Produce = false;
    private bool Something = true;
    private bool o = true;

    public bool Collect_Money = false;
    public GameObject Collect_Money_Icon;

    [Space]

    [Header("UI Upgrades etc.")]
    public bool UI_Open = false;
    public GameObject UI_Neigbourhood_Upgrades;
    public TextMeshProUGUI Power_Converting_Display;
    public TextMeshProUGUI production_Time_Display;
    public TextMeshProUGUI Money_Production_Display;

    public float Upgrade_UsageCost = 10f;
    public TextMeshProUGUI Upgrade_UsageCost_Display;

    public float Upgrade_PaymentCost = 25;
    public TextMeshProUGUI Upgrade_PaymentCost_Display;

    [Space]

    [Header("Values")]

    public float Money_In_Neighborhood = 0f;
    public float Production_Time = 5f;

    //public float upgrade_Energy_Usage_cost = 0f;
    //public float upgrade_Payment_cost = 0f;

    public float upgrade_Usage = 1f;
    public float upgrade_Payment = 1f;

    [Space]

    [Header("UI")]
    public bool HouseUI = false;
    GameObject UI;



    [Space]

    [Header("Progress Bar")]
    public GameObject ProgressBar;
    public GameObject Progress;

    [Space]

    [Header("Refrences")]
    public GameManagerScript gMS;
    public GameObject camera_Movement;

    public GameObject neighbourhood_TurnedOff;
    public GameObject neighbourhood_TurnedOn;

    [Space]

    [Header("Music and SFX")]
    [SerializeField] private AudioSource youAreBroke_Sfx;
    [SerializeField] private AudioSource buttonClick_Sfx;
    [SerializeField] private AudioSource collectMoney_Sfx;
    

    [Space]

    private bool startClick = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Money_In_Neighborhood >= 1f)
        {
            Collect_Money_Icon.SetActive(true);
            Collect_Money= true;
        }

        total_neighborhood_power_usage = (house_Energy_Usage * house_Count);
        total_neighborhood_payment = (house_Payment * total_neighborhood_power_usage);


        #region UI Varibles

        if (total_neighborhood_power_usage > gMS.power)
        {
            Power_Converting_Display.color = Color.red;
        }
        else
        {
            Power_Converting_Display.color = Color.white;
        }
        Power_Converting_Display.text = total_neighborhood_power_usage.ToString();
        production_Time_Display.text = Production_Time.ToString() + " sec.";
        Money_Production_Display.text = total_neighborhood_payment.ToString();

        Upgrade_UsageCost_Display.text = Upgrade_UsageCost.ToString();
        Upgrade_PaymentCost_Display.text = Upgrade_PaymentCost.ToString();

        if (gMS.money < Upgrade_PaymentCost)
        {
            Upgrade_PaymentCost_Display.color = Color.red;
        }
        else
        {
            Upgrade_PaymentCost_Display.color = Color.white;
        }


        if (gMS.money < Upgrade_UsageCost)
        {
            Upgrade_UsageCost_Display.color = Color.red;
        }
        else
        {
            Upgrade_UsageCost_Display.color = Color.white;
        }
        #endregion






        #region UI
        if (UI_Open == true)
        {
            camera_Movement.SetActive(false);
            UI_Neigbourhood_Upgrades.SetActive(true);
        }
        else if (UI_Open != true)
        {
            camera_Movement.SetActive(true);
            UI_Neigbourhood_Upgrades.SetActive(false);
        }
        #endregion


        if(gMS.power >= total_neighborhood_power_usage && Something == true)
        {
            Ready_to_Produce = false;
            Something = false;
        }
    }



    public void SellToNeighberhood()
    {
        if(gMS.power >= total_neighborhood_power_usage && Ready_to_Produce == false && o == true)
        {
            neighbourhood_TurnedOn.SetActive(true);
            neighbourhood_TurnedOff.SetActive(false);

            if (startClick == false)
            {
                buttonClick_Sfx.Play();
                startClick = true;
                o = false;
            }


            SellPowerToNeighborhood = true;
            AnimateBar();

            Ready_to_Produce = true;

            UI_Open = false;

            gMS.power -= total_neighborhood_power_usage;
        }
        else
        {
            youAreBroke_Sfx.Play();
        }
    }

    #region ProgressBar
    public void AnimateBar()
    {
        if(SellPowerToNeighborhood == true)
        ProgressBar.SetActive(true);
        LeanTween.scaleX(Progress, 0.95f, Production_Time).setOnComplete(PowerToMoney);

    }

    #endregion

    public void PowerToMoney()
    {
        Progress.transform.localScale = new Vector3(0f, 0.41f, 1f);
        Money_In_Neighborhood += total_neighborhood_payment;
        Collect_Money = true;
        Collect_Money_Icon.SetActive(true);
        if (Ready_to_Produce == true && gMS.power >= total_neighborhood_power_usage)
        {
            Ready_to_Produce = false;
            o = true;
            Invoke("SellToNeighberhood", 1f);
            
        }
        else
        {
            neighbourhood_TurnedOn.SetActive(false);
            neighbourhood_TurnedOff.SetActive(true);
            ProgressBar.SetActive(false);
            Something = true;
            o = true;
        }

    }




    private void OnMouseDown()
    {
        if (Collect_Money == true)
        {
            Collect_Money_Icon.SetActive(false);
            Collect_Money = false;
            gMS.money += Money_In_Neighborhood;
            Money_In_Neighborhood = 0f;

            //collectPower_Sfx.Play();
            

        }



    }


    public void StopSellingToNeighberhood()
    {
        SellPowerToNeighborhood = false;

        buttonClick_Sfx.Play();

        Ready_to_Produce = false;

        UI_Open = false;
        startClick = false;
    }


    public void Force_Stop_Neighbourhood()
    {
        Ready_to_Produce = false;
        startClick = false;
    }

    public void Exit_Neighbourhood_UI()
    {
        buttonClick_Sfx.Play();
        UI_Open = false;
    }


    public void Open_Neighbourhood_UI()
    {
        UI_Open = true;
    }
    public void CloseHouseUI()
    {
        UI.SetActive(false);
    }


    #region Upgrades

    public void UsageUpgrade()
    {

        if (gMS.money >= Upgrade_UsageCost)
        {
            gMS.money -= Upgrade_UsageCost;
            house_Energy_Usage += 1;
            Upgrade_UsageCost = Upgrade_UsageCost * 2;

        }
        else
        {
            youAreBroke_Sfx.Play();
        }
    }

    public void PaymentUpgrade()
    {
        
        if (gMS.money >= Upgrade_PaymentCost)
        {
            gMS.money -= Upgrade_PaymentCost;
            house_Payment += 1;
            Upgrade_PaymentCost = Upgrade_PaymentCost * 2;

        }
        else
        {
            youAreBroke_Sfx.Play();
        }
    }
    #endregion


}
