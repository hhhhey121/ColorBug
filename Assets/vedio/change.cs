using UnityEngine;
using UnityEngine.SceneManagement;

public class change : MonoBehaviour
{
    private void OnMouseDown()
    {
        // ��������������ʱ��������Ϊ "vedio" �ĳ���
        SceneManager.LoadScene("vedio");
    }
}
