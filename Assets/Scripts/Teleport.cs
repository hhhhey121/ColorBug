using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    //传送音效
    private AudioSource door;
    [SerializeField] private AudioClip doorSound;

    private GameObject currentTeleporter;

    void Start()
    {
        door = GetComponent<AudioSource>();

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentTeleporter!=null)
            {
                transform.position = currentTeleporter.GetComponent<Portal>().GetDestination().position;
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测玩家进入传送门触发区域
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if(collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
            
        }
    }

    
}

