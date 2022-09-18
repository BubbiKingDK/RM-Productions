using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudio : MonoBehaviour
{
    public bool Animation1 = false;

    [SerializeField] private AudioSource background_Music;
    public GameObject startAnimation;
    public GameObject startMainScreen;

    // Start is called before the first frame update
    void Start()
    {
        background_Music.Play();
        startAnimation.SetActive(true);
        Invoke("StartMain", 13.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMain()
    {
        if(Animation1== true)
        {
            startMainScreen.SetActive(true);
            Invoke("StopAnimation", 2f);
        }
       
    }

    public void StopAnimation()
    {
        startAnimation.SetActive(false);
    }
}
