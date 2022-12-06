using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 localRotation;
    bool cameraDisabled = false;
    

    
    void LateUpdate()
    {
      if(Input.GetMouseButtonDown(0))
        {
          if(!cameraDisabled)
            {
                localRotation.x += Input.GetAxis("Mouse X") * 15;
                localRotation.y += Input.GetAxis("Mouse Y") * -15;
                localRotation.y = Mathf.Clamp(localRotation.y, -90, 90);
            }

        }
        Quaternion qt = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation,qt,Time.deltaTime*15);
    }
}
