using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class vedioend : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // 拖入你的视频播放器
    public string nextSceneName = "chapter1";  // 播放完要切换的场景名

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // 当视频播放到结尾时触发事件
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
