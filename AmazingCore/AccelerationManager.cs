using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationManager : MonoBehaviour
{

    private void FixedUpdate()
    {
        float gx = Input.acceleration.x;
        float gy = Input.acceleration.y;
        float gz = Input.acceleration.z;

        Physics.gravity = new Vector3(gx, gy, gz);

    }
}
