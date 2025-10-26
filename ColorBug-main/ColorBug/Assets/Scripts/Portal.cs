using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal:MonoBehaviour
{
    [SerializeField] private Transform destination;//Ä¿µÄµØ

    public Transform GetDestination()
    {
        return destination;
    }
}