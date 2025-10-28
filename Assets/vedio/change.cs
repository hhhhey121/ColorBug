using UnityEngine;
using UnityEngine.SceneManagement;

public class change : MonoBehaviour
{
    private void OnMouseDown()
    {
        // 当点击到这个物体时，加载名为 "vedio" 的场景
        SceneManager.LoadScene("vedio");
    }
}
