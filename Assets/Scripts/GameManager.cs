using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameMode { forehand_practice, volley_practice, smash_practice }
    public GameMode selectedMode = GameMode.forehand_practice;

    public string whichMode = "Mirror";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void SetMirrorMode()
    {
        whichMode = "Mirror";
    }
    public void SetGameMode(GameMode mode)
    {
        whichMode = "Practice";
        selectedMode = mode;
    }
}

