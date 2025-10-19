using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//跟场景有关的要写

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;//结束音频 （后期添加
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
    private void finishLevel()//关卡结束
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
