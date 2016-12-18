using UnityEngine;
using System.Collections;

public class sensorRotator : MonoBehaviour {

    public bool rot_enabled;

    // Use this for initialization
    void Start()
    {
        enabled = false;
        Input.compensateSensors = true;
        Input.gyro.enabled = true;
    }

    void FixedUpdate()
    {
        if (rot_enabled)
        { 
            transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z);
        }
    }

    public void setOffSet()
    {
        transform.rotation = Quaternion.identity;
    }

    public void setRotationEnabled(bool enabled)
    {
        rot_enabled = enabled;
    }

}
