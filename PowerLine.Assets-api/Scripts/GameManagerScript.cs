using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{

    #region Variables
    [Header("Resources:")]

    [Header("Money:")]
    public float money;
    private float money_BackUp = 0f;
    private bool backUpMoney = false;
    public bool enoughMoney = false;
    public TextMeshProUGUI moneyDisplay;

    [Header("Windmill Power:")]
    public float Windmill_Count = 0f;
    public float windmill_Power_Generation_Time = 1f;
    public float windmill_Power_Generation_Amount = 1f;
    public float windmill_efficiency = 1;

    public float power;
    public TextMeshProUGUI powerDisplay;
    public float max_power_Capacity = 0f;

    [Header("Oil:")]
    public float oil;
    public TextMeshProUGUI oilDisplay;
    public float Oil_Rigs_Count = 0f;

    [Header("CO2:")]
    public float cO2 = 0f;
    public TextMeshProUGUI cO2Display;
    public bool cO2_100 = false;

    [Space]

    [Header("High Voltage Transformer")]
    public float HV_Transformer_Count = 0f;


    [Space]

    [Header("Unlock Buildings")]
    public GameObject unlock_Windmill;
    public GameObject unlock_Windmill_Background;

    [Space]

    [Header("Grid Building System:")]

    private Building buildingToPlace;

    public GameObject grid;
    public Tile[] tiles;
    public bool CloseEnough = false;
    private float close_Enough_To_Tile;

    [Space]

    [Header("UI")]

    public bool windmill_Active = false;

    public bool oil_Rig_Active = false;

    public bool HV_Transformer_Active = false;

    public GameObject buy_Menu;
    public GameObject shop_Button;
    public GameObject cancel_Building;
    public bool buy_Menu_Open = false;

    

    [Space]

    [Header("Custom Cursors")]
    public Custom_Cursor_Windmill customCursorWindmill;
    public Custom_Cursor_HighVoltageTransformer customCursorHVTransformer;
    public Custom_Cursor_Oil_Rig customCursorOil_Rig;

    public bool canBePlace = false;

    [Space]

    [Header("Cancel Building")]
    public bool esc_Building = true;

    [Space]

    [Header("Refrenes")]

    public GameObject canvas;
    public Factory fS;

    [Space]

    [Header("Music and SFX")]
    [SerializeField] private AudioSource background_Music;
    [SerializeField] private AudioSource buyBuilding_Sfx;
    [SerializeField] private AudioSource youAreBroke_Sfx;
    [SerializeField] private AudioSource placeBuilding_Sfx;
    [SerializeField] private AudioSource buttonClick_Sfx;
    #endregion




    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(true);
        BackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        #region Restart Scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        #endregion

        #region Cheats
        if (Input.GetKeyDown(KeyCode.M))
        {
            money += 9999999f;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            oil += 10f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            max_power_Capacity += 100f;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            power += 50f;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            cO2 += 100f;
        }

        #endregion


        #region Resources and money
        moneyDisplay.text = money.ToString();
        oilDisplay.text = oil.ToString();
        powerDisplay.text = power.ToString() + "/" + max_power_Capacity.ToString();
        cO2Display.text = cO2.ToString();

        #region Resouce amount color
        if (power == max_power_Capacity)
        {
            powerDisplay.color = Color.red;

            fS.Force_Stop_Factory();
        }
        else
        {
            powerDisplay.color = Color.white;
        }

        if (oil == 0)
        {
            oilDisplay.color = Color.red;
        }
        else
        {
            oilDisplay.color = Color.white;
        }

        if (money == 0)
        {
            moneyDisplay.color = Color.red;
        }
        else
        {
            moneyDisplay.color = Color.white;
        }
        #endregion
        #endregion

        #region Make Cursor Green If Placeable
        Tile nearestTile1 = null;
        float shortestDistance1 = float.MaxValue;
        foreach (Tile tile1 in tiles)
        {

            float dist1 = Vector2.Distance(tile1.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (dist1 < shortestDistance1)
            {
                shortestDistance1 = dist1;
                nearestTile1 = tile1;
            }

            close_Enough_To_Tile = shortestDistance1;

            if (close_Enough_To_Tile <= .25f)
            {
                CloseEnough = true;
            }
            else
            {
                CloseEnough = false;
            }

            if (enoughMoney == true && CloseEnough == true)
            {
                canBePlace = true;
            }
            else
            {
                canBePlace = false;
            }
        }
        #endregion

        #region Esc Cancel Build
        if (Input.GetKey(KeyCode.Escape))
        {

            ESCBuilding();
        }
        #endregion


        #region Building System
        if (Input.GetMouseButtonDown(0) && buildingToPlace != null && esc_Building == false)
        {

            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach (Tile tile in tiles)
            {
                float dist = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    nearestTile = tile;
                }

                close_Enough_To_Tile = shortestDistance;

            }

            if (nearestTile.isOccupied == false && shortestDistance <= .25f)
            {

                if (windmill_Active == true)
                {
                    PlaceBuildingSFX();
                    Instantiate(buildingToPlace, nearestTile.transform.position - new Vector3(x: .15f, y: -.7f, z: 0f), Quaternion.identity);
                    buildingToPlace = null;
                    nearestTile.isOccupied = true;
                    grid.SetActive(false);
                    customCursorWindmill.gameObject.SetActive(false);
                    Cursor.visible = true;

                    Windmill_Count++;

                    money_BackUp = 0f;
                    backUpMoney = false;

                    windmill_Active = false;

                    buy_Menu.SetActive(true);
                    shop_Button.SetActive(true);
                    cancel_Building.SetActive(false);


                    StartCoroutine(windmill_Power_Generation());



                }
                else if (oil_Rig_Active == true)
                {
                    PlaceBuildingSFX();
                    Instantiate(buildingToPlace, nearestTile.transform.position - new Vector3(x: 0f, y: -.4f, z: 0f), Quaternion.identity);
                    buildingToPlace = null;
                    nearestTile.isOccupied = true;
                    grid.SetActive(false);
                    customCursorOil_Rig.gameObject.SetActive(false);
                    Cursor.visible = true;

                    Oil_Rigs_Count++;

                    money_BackUp = 0f;
                    backUpMoney = false;

                    oil_Rig_Active = false;

                    buy_Menu.SetActive(true);
                    shop_Button.SetActive(true);
                    cancel_Building.SetActive(false);

                    GainResources_Oil();
                }

                else if (HV_Transformer_Active == true)
                {
                    PlaceBuildingSFX();
                    Instantiate(buildingToPlace, nearestTile.transform.position - new Vector3(x: 0f, y: -.4f, z: 0f), Quaternion.identity);
                    buildingToPlace = null;
                    nearestTile.isOccupied = true;
                    grid.SetActive(false);
                    customCursorHVTransformer.gameObject.SetActive(false);
                    Cursor.visible = true;

                    HV_Transformer_Count++;

                    money_BackUp = 0f;
                    backUpMoney = false;

                    HV_Transformer_Active = false;

                    buy_Menu.SetActive(true);
                    shop_Button.SetActive(true);
                    cancel_Building.SetActive(false);


                    HVTransformer();


                }

            }
        }
        #endregion

        
        #region Trigger Unlocks
        if (cO2 >= 500 && cO2_100 == false)
        {
            UnlockWindmill_();
            cO2_100 = true;

        }

        #endregion
    }

    public void BuyBuilding_Windmill(Building building)
    {
        if (money >= building.cost)
        {
            BuyBuildingSFX();
            esc_Building = false;
            enoughMoney = true;
            windmill_Active = true;

            customCursorWindmill.gameObject.SetActive(true);
            customCursorWindmill.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            money_BackUp = money;
            backUpMoney = true;

            money -= building.cost;
            buildingToPlace = building;
            grid.SetActive(true);

            buy_Menu.SetActive(false);
            shop_Button.SetActive(false);
            cancel_Building.SetActive(true);


        }
        else
        {
            YouAreBrokeSFX();
            enoughMoney = false;


        }
    }

    public void BuyBuilding_Oil_Rig(Building building)
    {
        if (money >= building.cost)
        {
            BuyBuildingSFX();
            esc_Building = false;
            enoughMoney = true;
            customCursorOil_Rig.gameObject.SetActive(true);
            customCursorOil_Rig.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            money_BackUp = money;
            backUpMoney = true;

            money -= building.cost;
            buildingToPlace = building;
            grid.SetActive(true);

            buy_Menu.SetActive(false);
            shop_Button.SetActive(false);
            cancel_Building.SetActive(true);
        }
        else
        {
            YouAreBrokeSFX();
            enoughMoney = false;


        }

    }
    public void BuyBuilding_HV_Transformer(Building building)
    {
        if (money >= building.cost)
        {
            BuyBuildingSFX();
            esc_Building = false;
            enoughMoney = true;
            customCursorHVTransformer.gameObject.SetActive(true);
            customCursorHVTransformer.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            money_BackUp = money;
            backUpMoney = true;

            money -= building.cost;
            buildingToPlace = building;
            grid.SetActive(true);

            buy_Menu.SetActive(false);
            shop_Button.SetActive(false);
            cancel_Building.SetActive(true);
        }
        else
        {
            YouAreBrokeSFX();
            enoughMoney = false;



        }
    }

    #region UI
    public void buyMenu()
    {
        if (buy_Menu_Open == true)
        {
            ButtonClickSFX();
            buy_Menu.SetActive(false);
            buy_Menu_Open = false;

        }
        else
        {
            ButtonClickSFX();
            buy_Menu.SetActive(true);
            buy_Menu_Open = true;



        }

    }
    #endregion

    #region Esc Cancel Build
    public void ESCBuilding()
    {    
       if (backUpMoney == true)
       {
          money = money_BackUp;
       }
       if (buy_Menu_Open != false)
       {
           customCursorWindmill.gameObject.SetActive(false);
           customCursorOil_Rig.gameObject.SetActive(false);
           customCursorHVTransformer.gameObject.SetActive(false);
           Cursor.visible = true;
           grid.SetActive(false);

           buy_Menu.SetActive(true);
           shop_Button.SetActive(true);
           cancel_Building.SetActive(false);

            esc_Building = true;

           windmill_Active = false;
           oil_Rig_Active = false;
           HV_Transformer_Active = false;
           
       }
        
    }
    #endregion



    #region Gain Resources
    public void GainResources_Oil()
    {
        if (Oil_Rigs_Count >= 1)
        {
            Invoke("GainOil", 5f);

        }

    }
    public void GainOil()
    {
        oil += 1;
        GainResources_Oil();
    }




    private IEnumerator windmill_Power_Generation()
    {
        if (power < max_power_Capacity)
        {
            power += windmill_Power_Generation_Amount * windmill_efficiency;
            yield return new WaitForSeconds(windmill_Power_Generation_Time);
            StartCoroutine(windmill_Power_Generation());
        }
        else
        {
            yield return new WaitForSeconds(windmill_Power_Generation_Time);
            StartCoroutine(windmill_Power_Generation());
        }

    }

    public void HVTransformer()
    {
        max_power_Capacity += 20f;
    }

    #endregion


    #region Unlock Buildings

    public void UnlockWindmill_()
    {
        unlock_Windmill.SetActive(true);
        unlock_Windmill_Background.SetActive(true);

    }

    #endregion



    #region Music and SFX

    public void BackgroundMusic()
    {
        background_Music.Play();
    }

    public void BuyBuildingSFX()
    {
        buyBuilding_Sfx.Play();
    }

    public void YouAreBrokeSFX()
    {
        youAreBroke_Sfx.Play();
    }

    public void PlaceBuildingSFX()
    {
        placeBuilding_Sfx.Play();
    }

    public void ButtonClickSFX()
    {
        buttonClick_Sfx.Play();
    }


    #endregion




}
