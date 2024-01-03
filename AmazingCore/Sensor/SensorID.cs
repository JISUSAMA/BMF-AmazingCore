using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorID : MonoBehaviour
{
    private Text sensorID;
    private void OnEnable()
    {
        sensorID = transform.GetComponent<Text>();
        sensorID.text = PlayerPrefs.GetString("AMC_DeviceAddr");
        SensorManager.instance.connectState += ConnectState;
    }

    private void OnDisable()
    {
        SensorManager.instance.connectState -= ConnectState;
    }

    private void ConnectState(SensorManager.States reciever)
    {
        Debug.Log($"ConnectState is {reciever} in SensorID");

        if (SensorManager.States.Scan == reciever || SensorManager.States.Connect == reciever)
        {
            sensorID.text = PlayerPrefs.GetString("AMC_DeviceAddr");
        }
    }
}
