//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

[DefaultExecutionOrder(-1)] //run before the other scripts
public class GameSystem : MonoBehaviour
{
    private static GameSystem instance;

    public delegate void OnGameRestart();
    public event OnGameRestart onGameRestart;

    [SerializeField] private HoverButton restartButton;

    public void OnRestartGameButton(Hand hand)
    {
        if (onGameRestart != null) onGameRestart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static GameSystem Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameSystem>();
            }
            return instance;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        restartButton.onButtonDown.AddListener(OnRestartGameButton);
        BGAudioManager.Instance.PlayMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
