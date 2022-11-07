using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
public class DetectPlayer : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            thirdPersonController = other.gameObject.GetComponent<ThirdPersonController>();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            thirdPersonController = null;
        }

    }
}
