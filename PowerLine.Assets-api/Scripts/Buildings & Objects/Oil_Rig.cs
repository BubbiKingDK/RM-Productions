using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.EventSystems;

public class Oil_Rig : MonoBehaviour
{



    [Header("Oil")]
    public float oil_Gain = 1f;
    public float Max_Oil = 50f;
    private float Oil_Production_Time = 4f;


    [Space]

    [Header("Progress Bar")]
    public GameObject ProgressBar;
    public GameObject Progress;

    [Space]

    public GameManagerScript gMS;

    [Space]

    [Header("Music and SFX")]
    [SerializeField] private AudioSource oil_Sfx;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("AnimateBar", 1f);
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void AnimateBar()
    {
        ProgressBar.SetActive(true);
        LeanTween.scaleX(Progress, 0.95f, Oil_Production_Time).setOnComplete(Oil_Gain);

    }

    public void Oil_Gain()
    {
        Progress.transform.localScale = new Vector3(0f, 0.41f, 1f);
        Invoke("AnimateBar", 1f);
    }

    private void OnMouseDown()
    {
        oil_Sfx.Play();
    }

}
