using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkCamera : NetworkBehaviour 
{
    private GameObject targetCam;
    private float yoffset = 8.4f;
    private Vector3 vOffset;
    //[SerializeField] private int camDelay = 1000;
    private Vector3 TopBoundary;
    private Vector3 BottomBoundary;

    // Start is called before the first frame update
    private void Start()
    {
        vOffset = new Vector3(0, yoffset, 0);
        targetCam = GameObject.FindGameObjectWithTag("MainCamera");
        TopBoundary = new Vector3(0, 3.234f, 0);
        BottomBoundary = new Vector3(0, -4.934f, 0);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        if (transform.position.y > TopBoundary.y)
        {
            targetCam.transform.position += vOffset;
            TopBoundary += vOffset;
            BottomBoundary += vOffset;
        }

        else if (transform.position.y < BottomBoundary.y)
        {
            targetCam.transform.position -= vOffset;
            TopBoundary -= vOffset;
            BottomBoundary -= vOffset;
        }
    }
}
