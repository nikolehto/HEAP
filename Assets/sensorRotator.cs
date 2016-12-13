using UnityEngine;
using System.Collections;

public class sensorRotator : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Input.compensateSensors = true;
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
    }

}
