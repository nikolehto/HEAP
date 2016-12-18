using UnityEngine;
using System.Collections;

public class cameraStretch : MonoBehaviour {

    public Matrix4x4 originalProjection;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.ResetProjectionMatrix();

        var m = cam.projectionMatrix;
        //Debug.Log(m.m00);
        m.m00 = m.m00/2; // width
        //m.m11 = 2; // height

        cam.projectionMatrix = m;
    }


}
