using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {
    Gyroscope m_gyroscope;

    // Use this for initialization
    void Start()
    {
        m_gyroscope = Input.gyro;
        m_gyroscope.enabled = true;
    }

    void OnGUI()
    {
        GUILayout.Label("Gyroscope attitude : " + m_gyroscope.attitude);
        GUILayout.Label("Gyroscope gravity : " + m_gyroscope.gravity);
        GUILayout.Label("Gyroscope rotationRate : " + m_gyroscope.rotationRate);
        GUILayout.Label("Gyroscope rotationRateUnbiased : " + m_gyroscope.rotationRateUnbiased);
        GUILayout.Label("Gyroscope updateInterval : " + m_gyroscope.updateInterval);
        GUILayout.Label("Gyroscope userAcceleration : " + m_gyroscope.userAcceleration);
    }
}
