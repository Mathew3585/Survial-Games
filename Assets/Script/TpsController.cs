using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class TpsController : MonoBehaviour
{
    [SerializeField] private Rig aimrig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private ProjectileGun projectileGun;
    public bool WeaponEquip;


    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private StarterAssetsInputs starterAssets;
    [SerializeField] private Animator animator;
    private float aimRigWeight;
    // Start is called before the first frame update
    void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssets = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        projectileGun = GetComponent<ProjectileGun>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPostion = Vector3.zero;
        Vector2 screeCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screeCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPostion = raycastHit.point;
        }
        if (starterAssets.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            aimRigWeight = 1f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f , Time.deltaTime * 10f));

            Vector3 WorldAimTratget = mouseWorldPostion;
            WorldAimTratget.y = transform.position.y;
            Vector3 aimDirection = (WorldAimTratget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }   
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            aimRigWeight = 0f;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
        aimrig.weight = Mathf.Lerp(aimrig.weight, aimRigWeight, Time.deltaTime * 20f);

        if(WeaponEquip == true)
        {
            if (projectileGun.reloading)
            {
                animator.SetLayerWeight(2, 1f);
            }
            else
            {
                animator.SetLayerWeight(2, 0f);
            }
        }
        else
        {
            return;
        }

    }



}
