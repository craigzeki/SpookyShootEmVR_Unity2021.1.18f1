using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LimitFrameRate : MonoBehaviour
{
    [SerializeField] TextMeshPro FPSText1;
    [SerializeField] TextMeshPro FPSText2;
    [SerializeField] bool fixFPS = false;
    [SerializeField] int targetFPS = 60;

    private int minFPS = 200;
    private int maxFPS = 0;
    private float timeDelay = 2.0f;
    private float timeElapsed = 0f;

    private void Awake()
    {
        if(fixFPS == true)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= timeDelay)
        {
            minFPS = 200;
            maxFPS = 0;
            timeElapsed = 0f;
        }
        int curFPS = ((int)Mathf.Round((1.0f / Time.deltaTime)));

        if (curFPS < minFPS) minFPS = curFPS;
        if (curFPS > maxFPS) maxFPS = curFPS;

        //FPSText1.SetText(("FPS: " + Mathf.Round((1.0f / Time.deltaTime))).ToString());
        FPSText1.SetText(minFPS.ToString() + " : " + curFPS.ToString() + " : " + maxFPS.ToString());
        
        FPSText2.SetText(FPSText1.text);
        if((Application.targetFrameRate != targetFPS) && (fixFPS == true))
        {
            Application.targetFrameRate = targetFPS;
        }
    }
}