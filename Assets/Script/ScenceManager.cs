using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ScenceManager : NetworkBehaviour 
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float yOffset = 8;
    //[SerializeField] private GameObject target;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 vec3 = new Vector3(0, yOffset, 0);
        if (collision.tag == "Player")
        {
            Debug.Log("Scene change");
            mainCamera.transform.position += vec3; 
        }
    }
}
