using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject ModeSelectPanel;
    public GameObject PoseSelectPanel;

    public string whichMode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 연습모드 선택 시 자세 선택으로 넘어감.
    /// </summary>
    public void OnPracticeSelect()
    {
        ModeSelectPanel.SetActive(false);
        PoseSelectPanel.SetActive(true);
    }

    /// <summary>
    /// 거울모드 선택 시 바로 거울 연습장으로 넘어감.
    /// </summary>
    public void OnMirrorSelect()
    {
        ModeSelectPanel.SetActive(false);
        GameManager.instance.SetMirrorMode();
        SceneManager.LoadScene("MirrorScene"); // 연습모드 씬 로드
    }

    /// <summary>
    /// 포즈 선택 시 선택 정보가 다음 씬으로 넘어감. (GameManager의 DontDestroyOnLoad 이용)
    /// </summary>
    /// <param name="modeIndex"></param>
    public void OnPoseSelect(int modeIndex)
    {
        GameManager.instance.SetGameMode((GameManager.GameMode)modeIndex);
        SceneManager.LoadScene("PracticeScene"); // 연습모드 씬 로드
    }
}
