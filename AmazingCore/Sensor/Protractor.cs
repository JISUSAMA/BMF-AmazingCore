using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Protractor : MonoBehaviour
{
    // Y 축 기준으로 각도 계산
    public GameObject secondHands;
    public Text angle;
    private Coroutine loop_coroutine;
    Transform cube;

    private void OnEnable()
    {
        Debug.Log("Protractor OnEnable");
        cube = SensorManager.instance.cube;
    }

    private void OnDisable()
    {
        Debug.Log("Protractor OnDisable");
        cube = null;
    }

    private void Update()
    {
        if (!SensorManager.instance._connected) { return; }

        // z 축 rotation
        if (cube.rotation.eulerAngles.y <= 90 && cube.rotation.eulerAngles.y >= 0)
        {
            secondHands.transform.eulerAngles = new Vector3(0f, 0f, cube.rotation.eulerAngles.y);
            angle.text = Math.Truncate(secondHands.transform.rotation.z).ToString();
        }
        else if (cube.rotation.eulerAngles.y <= 360 && cube.rotation.eulerAngles.y >= 270)
        {
            secondHands.transform.eulerAngles = new Vector3(0f, 0f, cube.rotation.eulerAngles.y);
            angle.text = Math.Truncate(secondHands.transform.rotation.z).ToString();
        }
        else
        {
            // 이외 각도 범위
            Debug.Log("angle y : " + cube.rotation.eulerAngles.y);
        }
    }

    //public void OnStartProtractCalc()
    //{
    //    if (!SensorManager.instance._connected)
    //    {
    //        Debug.Log("아직 연결안됌");
    //        return;
    //    }
    //    else
    //    {
    //        loop_coroutine = StartCoroutine(_OnStartProtractCalc());
    //    }
    //}

    //IEnumerator _OnStartProtractCalc()
    //{
    //    WaitForSeconds ws = new WaitForSeconds(0.3f);

    //    while (true)
    //    {
    //        Debug.Log("degree : " + (360f - cube.rotation.eulerAngles.x));
    //        // x : 360 - 270
    //        // y : 0 - 90
    //        if (cube.rotation.eulerAngles.x <= 360f && cube.rotation.eulerAngles.x >= 180f)
    //        {
    //            // 각도 계산 : 0 ~ 180
    //            secondHands.transform.eulerAngles = new Vector3(0f, 0f, 360f - cube.rotation.eulerAngles.x);
    //        }

    //        yield return ws;
    //    }
    //}

    //public void OnStopProtractCalc()
    //{
    //    StopCoroutine(loop_coroutine);
    //}
}
