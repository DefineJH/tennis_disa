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
    public Text detailText;
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
                text.text = "Forehand 기본 자세";
                detailText.text = "Step 1: 라켓을 든 방향의 발을 45도 방향으로 나가면서 허리를 틀어 라켓 면이 옆면이 보이도록 한다." +
                    "\nStep 2: 반대쪽 발을 앞으로 가져오면서 라켓을 뒤로 뺀다. 이때, 라켓을 잡지 않은 손은 다가오는 공을 가리킨다." +
                    "\nStep 3: 허리를 돌리면서 라켓을 앞으로 끌고 나온다. 이때 라켓은 지면과 수직이 되게 한다." +
                    "\nStep 4: 라켓을 어깨 너머로 넘기면서 팔로우 스윙을 한다. 이때 팔꿈치가 턱 밑까지 올 수 있도록 들어준다.";
                break;
            case GameManager.GameMode.volley_practice:
                player.clip = volleyClip;
                text.text = "Volley 기본 자세";
                detailText.text = "Step 1: 라켓을 든 방향의 발을 45도 방향으로 나가면서 라켓의 면은 네트와 수평이 되도록 만든다." +
                    "\nStep 2: 손목을 고정시킨채 공을 위에서 아래로 쓸어준다는 느낌으로 공을 맞추며 공이 맞는 순간 라켓을 배꼽 쪽으로 가져간다.";
                break;
            case GameManager.GameMode.smash_practice:
                player.clip = smashClip;
                text.text = "Smash 기본 자세";
                detailText.text = "Step 1: 라켓을 잡지 않은 손으로 하늘 위에 있는 공을 가리키며 라켓은 어깨 밑으로 내린다." +
                    "\nStep 2: 허리를 들어 몸통이 정면을 보게 만든다. 이때 라켓 헤드는 바닥을 보게 떨군다." +
                    "\nStep 3: 라켓을 멀리 던지듯이 휘두른다. 이때 공이 맞는 시점은 가장 높이 있을 때이며, 지면과 라켓이 수직일 때이다." +
                    "\nStep 4: Step 3에서 던진 라켓을 멈추지 않고 반대편 허리까지 끌고 온다. 이때 몸통은 숙여주는 것이 좋다.";
                break;
        }
        player.loopPointReached += endVideo;
        player.Play();
    }
    void endVideo(VideoPlayer vp)
    {
        if(startBtn != null /*&& !startBtn.gameObject.activeSelf*/) {
            startBtn.interactable = true;
        }
    }

    public void movePractice()
    {
        SceneManager.LoadScene("PracticeScene");
    }

}
