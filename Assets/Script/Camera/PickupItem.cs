using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PickupItem : MonoBehaviour
{
    public float pickupRange = 2.6f;
    public StarterAssetsInputs starterAssets;
    public PickupBehaviour playerPickup;

    [SerializeField]
    private GameObject pickuptext;

    [SerializeField]
    private LayerMask ItemMask;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position , transform.forward , out hit , pickupRange, ItemMask))
        {
            Debug.Log("ok");
            if (hit.transform.CompareTag("Item"))
            {
                pickuptext.SetActive(true);
                if (starterAssets.PickUp)
                {
                    playerPickup.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }

        }
        else
        {
            pickuptext.SetActive(false);
        }
    }
}
