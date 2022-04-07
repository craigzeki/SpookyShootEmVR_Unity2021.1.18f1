//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LimitFrameRate : MonoBehaviour
{
    //paramters for LimitFrameRate
    [SerializeField] TextMeshPro FPSText1;
    [SerializeField] TextMeshPro FPSText2;
    [SerializeField] bool fixFPS = false;
    [SerializeField] int targetFPS = 60;

    //first method
    //private int minFPS = 200;
    //private int maxFPS = 0;
    //private float timeDelay = 2.0f;

    //second method
    private int frameCount = 0;
    private int framesPerSecond = 0;
    [SerializeField] private float averageDuration = 2.0f; //2s

    //common between methods
    private float timeElapsed = 0f;

    private void Awake()
    {
        //try and force the framerate
        if(fixFPS == true)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        //old method, min, max and current but not smoothed / averaged
        //timeElapsed += Time.deltaTime;
        //if(timeElapsed >= timeDelay)
        //{
        //    minFPS = 200;
        //    maxFPS = 0;
        //    timeElapsed = 0f;
        //}
        //int curFPS = ((int)Mathf.Round((1.0f / Time.deltaTime)));

        //if (curFPS < minFPS) minFPS = curFPS;
        //if (curFPS > maxFPS) maxFPS = curFPS;

        ////FPSText1.SetText(("FPS: " + Mathf.Round((1.0f / Time.deltaTime))).ToString());
        //FPSText1.SetText(minFPS.ToString() + " : " + curFPS.ToString() + " : " + maxFPS.ToString());

        //FPSText2.SetText(FPSText1.text);

        
        frameCount++;
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= averageDuration)
        {
            framesPerSecond = (int)((float)frameCount / timeElapsed);
            frameCount = 0;
            timeElapsed = 0;
        }

        FPSText1.SetText(framesPerSecond + " FPS");
        if(FPSText2 != null) FPSText2.SetText(FPSText1.text);


        //reinforce the target framerate
        if((Application.targetFrameRate != targetFPS) && (fixFPS == true))
        {
            Application.targetFrameRate = targetFPS;
        }
    }
}
