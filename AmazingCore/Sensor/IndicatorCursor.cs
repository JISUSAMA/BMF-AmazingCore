using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCursor : MonoBehaviour
{
    public GameObject cursor;

    private Vector3 nextCursor;
    [HideInInspector] public static float distance;
    public float maxRadius;

    Transform cube;

    private Vector3 xyPos;
    private float xPos;
    private float yPos;

    private void OnEnable()
    {
        Debug.Log("IndicatorCursor OnEnable");
        cube = SensorManager.instance.cube;
    }

    private void OnDisable()
    {
        Debug.Log("IndicatorCursor OnDisable");
        cube = null;
    }

    /// <summary>
    /// 현재 위치가 0,0 으로부터 원바깥으로 안나가게 하기
    /// heading = cursor.transform.localPosition;
    /// distance = heading.magnitude;
    /// direction = heading / distance; // This is now the normalized direction.        

    /// Debug.Log("heading : " + heading);
    /// Debug.Log("distance : " + distance);
    /// Debug.Log("direction : " + direction);
    /// </summary>

    void Update()
    {
        if (!SensorManager.instance._connected) { return; }

        if (cube.rotation.eulerAngles.x <= 360 && cube.rotation.eulerAngles.x >= 270)
        {
            // 0 ~ -90
            xPos = cube.rotation.eulerAngles.x - 360;
        }
        else
        {
            xPos = cube.rotation.eulerAngles.x;
        }

        if (cube.rotation.eulerAngles.y <= 360 && cube.rotation.eulerAngles.y >= 270)
        {
            // 0 ~ -90
            yPos = cube.rotation.eulerAngles.y - 360;
        }
        else
        {
            yPos = cube.rotation.eulerAngles.y;
        }

        xyPos.x = xPos;
        xyPos.y = yPos;

        // xyPos x , y > -90 ~ 0 ~ 90        
        // cursor x, y -200 ~ 200

        //cursor.transform.localPosition <= xyPos 값 변환시켜서 옮겨줌
        var xRatio = Mathf.InverseLerp(-90, 90, xyPos.x);   // 0 ~ 1
        var xval = Mathf.Lerp(-maxRadius, maxRadius, xRatio);

        var yRatio = Mathf.InverseLerp(-90, 90, xyPos.y);
        var yval = Mathf.Lerp(-maxRadius, maxRadius, yRatio);

        nextCursor = new Vector3(xval, yval);
        distance = nextCursor.magnitude;

        if (distance < maxRadius)
        {
            cursor.transform.localPosition = new Vector3(xval, yval);
            nextCursor = new Vector3(xval, yval);
        }
        else
        {
            nextCursor = cursor.transform.localPosition;
        }
    }
}