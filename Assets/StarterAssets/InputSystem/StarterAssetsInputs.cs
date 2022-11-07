using System.Linq.Expressions;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public Inventory inventory;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool Shoot;
		public bool PickUp;
		public bool Reload;
		public bool OpenInventory;
		public bool WeaponHand;


		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}

		public void OnPickUp(InputValue value)
        {
			PickUpInput(value.isPressed);
		}
		public void OnReload(InputValue value)
        {
			RealoadInput(value.isPressed);
		}
		public void OnOpenInventory(InputValue value)
		{
			OpenInventoryInput(value.isPressed);

		}


#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

		public void ShootInput (bool newShootState)
		{
			Shoot = newShootState;
		}
		public void PickUpInput(bool newPickupState)
		{
			PickUp = newPickupState;
		}

		public void RealoadInput(bool newPickupState)
		{
			Reload = newPickupState;
		}
		public void OpenInventoryInput(bool newPickupState)
		{
			OpenInventory = newPickupState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}


        public void FixedUpdate()
        {
			if (OpenInventory == true)
			{
				aim = false;
				Shoot = false;
			}
			if(WeaponHand == false)
			{
                aim = false;
                Shoot = false;
            }
		}
    }	
}