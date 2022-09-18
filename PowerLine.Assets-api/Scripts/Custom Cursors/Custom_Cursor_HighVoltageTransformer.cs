using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Cursor_HighVoltageTransformer : MonoBehaviour
{
    public GameManagerScript gMS;

    [SerializeField] private float offsettY = 0f;
    [SerializeField] private float offsettX = 0f;


    public SpriteRenderer Cursor_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offsetCustomCursor = new Vector2(offsettX, offsettY); 
        transform.position = mousePosition -offsetCustomCursor;
        

        if(gMS.canBePlace == true )
        {
            Cursor_SpriteRenderer.color = new Color(0f, 255f, 0f, .5f);
        }
        else
        {
            Cursor_SpriteRenderer.color = new Color(255f, 0f, 0f, .5f);
        }
    }

    public void Activate_High_Voltage_Transformer()
    {
        gMS.HV_Transformer_Active = true;
    }
}
