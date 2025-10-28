using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class vedioend : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // ���������Ƶ������
    public string nextSceneName = "chapter1";  // ������Ҫ�л��ĳ�����

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // ����Ƶ���ŵ���βʱ�����¼�
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
