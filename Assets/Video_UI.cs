using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Video_UI : MonoBehaviour
{

    VideoPlayer player;
    public RawImage image;
    public RenderTexture renderTex;
    public Button startBtn;
    public VideoClip forehandClip;
    public VideoClip smashClip;
    public VideoClip volleyClip;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        var curMode = GameManager.instance.selectedMode;
        Debug.Log(curMode);
        switch (curMode)
        {
            case GameManager.GameMode.forehand_practice:
                player.clip = forehandClip;
                text.text = "설명(Forehand)";
                break;
            case GameManager.GameMode.volley_practice:
                player.clip = volleyClip;
                text.text = "설명(Volley)";
                break;
            case GameManager.GameMode.smash_practice:
                player.clip = smashClip;
                text.text = "설명(Smash)";
                break;
        }
        player.loopPointReached += endVideo;
        player.Play();
    }
    void endVideo(VideoPlayer vp)
    {
        if(startBtn != null && !startBtn.gameObject.activeSelf) {
            startBtn.gameObject.SetActive(true);
        }
    }

    public void movePractice()
    {
        SceneManager.LoadScene("PracticeScene");
    }

}
