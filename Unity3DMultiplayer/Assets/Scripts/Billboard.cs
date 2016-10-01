using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    private Camera camera = null;
    void Update()
    {
        if(camera == null)
            camera = FindObjectOfType<Camera>();

        if(camera != null)
            transform.LookAt(camera.transform);
    }
}