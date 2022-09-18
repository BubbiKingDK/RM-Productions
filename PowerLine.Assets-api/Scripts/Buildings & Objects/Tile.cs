using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isOccupied;
    public Color greenColor;
    public Color redColor;

    private SpriteRenderer rend;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isOccupied == true)
        {
            rend.color = redColor;
        }
        else
        {
            rend.color = greenColor;
        }
    }
}
