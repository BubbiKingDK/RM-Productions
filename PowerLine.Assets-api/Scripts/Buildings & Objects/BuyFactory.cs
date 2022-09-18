using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFactory : MonoBehaviour
{

    public GameObject destroyed_Factory;
    public GameObject Factory;
    public GameObject shop;

    public GameObject hitBox;

    [SerializeField] public float cost = 500f;
    [SerializeField] private bool bught = false;

    [SerializeField] private AudioSource factoryRestore_Sfx;

    public GameManagerScript gMS;
    public Factory fS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy()
    {
        if (gMS.money >= cost && bught == false)
        {

            factoryRestore_Sfx.Play();
            destroyed_Factory.SetActive(false);
            Factory.SetActive(true);
            gMS.money -= cost;
            bught = true;
            shop.SetActive(true);
            hitBox.SetActive(false);
            Invoke("OpenUI", 1);
        }
    }

    public void OpenUI()
    {
        fS.UI_Open = true;
    }
}
