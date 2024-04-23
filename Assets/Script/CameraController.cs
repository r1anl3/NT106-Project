using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour 
{

    [SerializeField] private Camera targetCam;
    private float yoffset = 8.4f;
    private Vector3 vOffset;
    //[SerializeField] private int camDelay = 1000;
    [SerializeField] private BoxCollider2D TopBoundary;
    [SerializeField] private BoxCollider2D BottomBoundary;

    // Start is called before the first frame update
    private void Start()
    {
        vOffset = new Vector3(0, yoffset, 0);
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Top")) 
        {
            Debug.Log("Top");
            targetCam.transform.position += vOffset;
            Thread.Sleep(camDelay);
        }

        else if (other.CompareTag("Bot") )
        {
            Debug.Log("Bot");
            targetCam.transform.position -= vOffset;
            Thread.Sleep(camDelay);
        }
    }
    */
    private void FixedUpdate()
    {
        if (transform.position.y > TopBoundary.bounds.center.y)
        {
            Debug.Log("Top");
            targetCam.transform.position += vOffset;
        }

        else if (transform.position.y < BottomBoundary.bounds.center.y)
        {
            Debug.Log("Bot");
            targetCam.transform.position -= vOffset;
        }
    }
}
