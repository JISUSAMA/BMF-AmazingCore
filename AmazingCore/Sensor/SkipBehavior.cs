using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipBehavior : MonoBehaviour
{
    public GameObject tutoGroup;
    public GameObject[] tutorial;
    public void toturialDisable()
    {
        Debug.Log("toturialDisable");
        for (int i = 0; i < tutorial.Length; i++)
        {
            Debug.Log("toturialDisable : " + tutorial[i]);
            tutorial[i].SetActive(false);
        }

        tutoGroup.SetActive(false);
    }
}
