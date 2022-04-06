using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call : MonoBehaviour
{
    public GameObject List;
    private void Awake()
    {
        Instantiate(List);
    }
}