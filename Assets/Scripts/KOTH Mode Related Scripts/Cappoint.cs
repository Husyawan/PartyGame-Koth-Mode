using UnityEngine;
using System;
using System.Collections.Generic;

public class CapPoint : MonoBehaviour
{
    public string CapturePoint_Name;
    private CapPointManager capPointManager;
    private bool isRedPlayerPresent = false;
    private bool isBluePlayerPresent = false;
    void Start()
    {
        capPointManager = FindObjectOfType<CapPointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedPlayer") || other.CompareTag("BluePlayer"))
        {
            PlayerCappedPoint player = other.GetComponent<PlayerCappedPoint>();
            if (other.CompareTag("RedPlayer"))
            {
                isRedPlayerPresent = true;
               
            }
            else if (other.CompareTag("BluePlayer"))
            {
                isBluePlayerPresent = true;
            }
            if (isRedPlayerPresent && isBluePlayerPresent)
            {
                // Stop capturing if both players are present
                capPointManager.StopCapturing(null, CapturePoint_Name);
            }
            else if (!capPointManager.isCapturing && !player.cappedpointlist.Contains(CapturePoint_Name))
            {
                capPointManager.StartCapturing(other.gameObject, CapturePoint_Name);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag("RedPlayer"))
        {
            isRedPlayerPresent = false;
        }
        else if (other.CompareTag("BluePlayer"))
        {
            isBluePlayerPresent = false;
        }

        // Stop capturing if either player leaves
        if (other.gameObject == capPointManager.activePlayer)
        {
            capPointManager.StopCapturing(other.gameObject, CapturePoint_Name);
        }
    }
}
