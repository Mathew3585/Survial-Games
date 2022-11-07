using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    private Animator animator;
    private ThirdPersonController personController;
    private Item currentItem;


    private void Start()
    {
        animator = GetComponent<Animator>();
        personController = GetComponent<ThirdPersonController>();
    }

    public void DoPickup(Item item)
    {
        if (inventory.IsFull())
        {
            Debug.Log("Inventaire Plien" + item.name);
            return;
        }

        currentItem = item;
        animator.SetBool("PickUp",true);
        personController.canMove = false;

    }


    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemdata);
        Destroy(currentItem.gameObject);
    }

    public void ReEnablePlayerMove()
    {
        animator.SetBool("PickUp", false);
        personController.canMove = true;
    } 
}
