using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal:MonoBehaviour
{
    [SerializeField] private Transform destination;//Ŀ�ĵ�

    public Transform GetDestination()
    {
        return destination;
    }
}