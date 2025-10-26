using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//�������йص�Ҫд

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;//������Ƶ ���������
    void Start()
    {
        finishSound=GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="Player")
        {
            finishSound.Play();
            finishLevel();
        }
    }
    private void finishLevel()//�ؿ�����
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
