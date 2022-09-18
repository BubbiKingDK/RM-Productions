using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Factory : MonoBehaviour
{

    

    [Header("On/Off")]
    public bool on;
    public GameObject factory_On;
    public GameObject factory_Off;
    [Space]

    [Header("Power Generation")]
    public bool Ready_to_Produce = false;
    private bool o = true;

    public bool Factory_Turned_ON = false;
    public float Power_In_Factory = 0f;
    public float Convert_Oil_Amount = 1f;
    public float Max_Power_Capacity = 20f;
    public float Production_Time = 5f;
    public float efficiency = 1f;
    public float power_Production;
    
    public bool Collect_Power = false;
    public GameObject Collect_Power_Icon;

    public float cO2 = 0f;
    public float total_CO2 = 0f;
    [Space]

    [Header("UI Upgrades etc.")]
    public bool UI_Open = false;
    public GameObject UI_Factory_Upgrades;
    public TextMeshProUGUI Oil_Converting_Display;
    public TextMeshProUGUI production_Time_Display;
    public TextMeshProUGUI power_Production_Display;
    public TextMeshProUGUI cO2_Realease_Display;

    public float Upgrade_OilTankCost = 10f;
    public TextMeshProUGUI Upgrade_OilTank_Display;

    public float Upgrade_GeneratorCost = 25f;
    public TextMeshProUGUI Upgrade_Generator_Display;

    [Space]

    [Header("Progress Bar")]
    public GameObject ProgressBar;
    public GameObject Progress;

    [Space]

    [Header("Music and SFX")]
    [SerializeField] private AudioSource youAreBroke_Sfx;
    [SerializeField] private AudioSource buttonClick_Sfx;
    [SerializeField] private AudioSource collectPower_Sfx;
    [SerializeField] private AudioSource tooMuchPower_Sfx;

    private bool startClick = false;

    [Space]
    [Header("Refrences")]
    public GameManagerScript gMS;
    public GameObject camera_Movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        total_CO2 = cO2 * Convert_Oil_Amount;
        if (Power_In_Factory >= 1f)
        {
            Collect_Power_Icon.SetActive(true);
            Collect_Power = true;
        }

        power_Production = Convert_Oil_Amount * efficiency;


        #region UI Varibles


        if (Convert_Oil_Amount > gMS.oil)
        {
            Oil_Converting_Display.color = Color.red;
        }
        else
        {
            Oil_Converting_Display.color = Color.white;
        }
        Oil_Converting_Display.text = Convert_Oil_Amount.ToString();
        production_Time_Display.text = Production_Time.ToString() + " sec.";
        power_Production_Display.text = power_Production.ToString();
        cO2_Realease_Display.text = total_CO2.ToString() + " CO2";

        Upgrade_Generator_Display.text = Upgrade_GeneratorCost.ToString();
        Upgrade_OilTank_Display.text = Upgrade_OilTankCost.ToString();

        if(gMS.money < Upgrade_GeneratorCost)
        {
            Upgrade_Generator_Display.color = Color.red;
        }
        else
        {
            Upgrade_Generator_Display.color = Color.white;
        }


        if (gMS.money < Upgrade_OilTankCost)
        {
            Upgrade_OilTank_Display.color = Color.red;
        }
        else
        {
            Upgrade_OilTank_Display.color = Color.white;
        }

        #endregion


        #region Factory on/off
        if (Factory_Turned_ON == true)
        {
            factory_On.SetActive(true);
            factory_Off.SetActive(false);
        }
        else if(Factory_Turned_ON != true)
        {
            factory_Off.SetActive(true);
            factory_On.SetActive(false);
        }
        #endregion

        #region UI
        if (UI_Open == true)
        {
            camera_Movement.SetActive(false);
            UI_Factory_Upgrades.SetActive(true);
        }
        else if (UI_Open != true)
        {
            camera_Movement.SetActive(true);
            UI_Factory_Upgrades.SetActive(false);
        }
        #endregion
    }



    private void OnMouseDown()
    {
        if (Collect_Power == true)
        {
            if (Power_In_Factory + gMS.power <= gMS.max_power_Capacity)
            {
                Collect_Power_Icon.SetActive(false);
                Collect_Power = false;
                gMS.power += Power_In_Factory;
                Power_In_Factory = 0f;

                collectPower_Sfx.Play();
            }
            else
            {
                float power_to_Max = gMS.max_power_Capacity - gMS.power;
                Power_In_Factory -= power_to_Max;
                gMS.power += power_to_Max;
                tooMuchPower_Sfx.Play();
                if (power_to_Max >= 1)
                {
                    collectPower_Sfx.Play();
                }
                else
                {
                    tooMuchPower_Sfx.Play();
                }

            }

        }
        else
        {
            UI_Factory_Upgrades.SetActive(true);
            UI_Open = true;
        }


    }

    #region Production
    public void Start_Factory()
    {
      if (gMS.oil >= Convert_Oil_Amount && Power_In_Factory < Max_Power_Capacity && Ready_to_Produce == false && o == true)
      {
           if(startClick == false)
            {
                buttonClick_Sfx.Play();
                startClick = true;
                o = false;
            }

           Ready_to_Produce = true;

           Factory_Turned_ON = true;

           UI_Open = false;

           gMS.oil -= Convert_Oil_Amount;
           AnimateBar();



      }
      else
      {
            youAreBroke_Sfx.Play();
      }
    }
    #region ProgressBar
    public void AnimateBar()
    {
        ProgressBar.SetActive(true);
        LeanTween.scaleX(Progress, 0.95f, Production_Time).setOnComplete(Oil_to_Power);

    }

    #endregion

    public void Oil_to_Power()
    {
        Progress.transform.localScale = new Vector3(0f, 0.41f, 1f);
        Power_In_Factory += power_Production;
        Collect_Power = true;
        Collect_Power_Icon.SetActive(true);
        gMS.cO2 += total_CO2;
        if (Power_In_Factory < Max_Power_Capacity && Ready_to_Produce == true)
        {
            Ready_to_Produce = false;
            o = true;
            Invoke("Start_Factory", 1f);
            
        }
        else
        {
            Factory_Turned_ON = false;
            o = true;

        }

    }

    #endregion




    public void Stop_Factory()
    {
        buttonClick_Sfx.Play();

        Ready_to_Produce = false;

        UI_Open = false;
        startClick = false;


    }

    public void Force_Stop_Factory()
    {
        Ready_to_Produce = false;
        startClick = false;


    }

    public void Exit_Upgrade_UI()
    {
        buttonClick_Sfx.Play();
        UI_Open = false;
    }


    #region Upgrades

    public void OilTankUpgrade()
    {
        float UpgradeCount_1 = 0;
        if (gMS.money >= Upgrade_OilTankCost)
        {
            gMS.money -= Upgrade_OilTankCost;
            UpgradeCount_1 += 1;
            
            if(UpgradeCount_1 == 5)
            {
                Convert_Oil_Amount += Convert_Oil_Amount / 2;
                UpgradeCount_1 = 0;
            }
            else
            {
                Convert_Oil_Amount += 5;
            }

            Upgrade_OilTankCost = Upgrade_OilTankCost * 2;
        }
        else
        {
            youAreBroke_Sfx.Play();
        }
    }

    public void GeneratorUpgrade()
    {
        float UpgradeCount_2 = 0;
        if (gMS.money >= Upgrade_GeneratorCost)
        {
            gMS.money -= Upgrade_GeneratorCost;
            UpgradeCount_2 += 1;
            efficiency += 1;
            Upgrade_GeneratorCost = Upgrade_GeneratorCost * 2;

        }
        else
        {
            youAreBroke_Sfx.Play();
        }
    }
    #endregion


}
