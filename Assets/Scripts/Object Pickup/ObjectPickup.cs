using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] private float _pickupDistance = 3f;
    [SerializeField] private float _throwForce = 10f;

    private DroppableComponent _heldDroppable;
    public Camera playerCamera;

    private StarterAssets.StarterAssetsInputs _input;
    InputAction _pickupAction;
    InputAction _throwAction;


    private void OnEnable()
    {
     //   _pickupAction.Enable();
    }

    private void OnDisable()
    {
       // _pickupAction.Disable();
    }

    private void Awake()
    {
        _input = GetComponent<StarterAssets.StarterAssetsInputs>();

       /* _pickupAction = new InputAction("Pickup", InputActionType.Button, "<Keyboard>/e");
        _pickupAction.performed += ctx => TryPickup();
        _pickupAction.Enable();
        _throwAction = new InputAction("Throw", InputActionType.Button, "<Keyboard>/f");
        _throwAction.performed += ctx => ThrowObject();
        _throwAction.Enable();*/
    }

    private void FixedUpdate()
    {
        if (_input.pickup)
        {
            TryPickup();
        }
        MoveHeldDroppable();
    }
    void MoveHeldDroppable()
    {
        if (_heldDroppable != null)
        {
            Transform main = playerCamera.transform;
            // Move the held object in front of the player
            Vector3 heldObjectPosition = main.position + main.forward * _pickupDistance - main.up * _pickupDistance * .2f;
            _heldDroppable.rigidbody.MovePosition(heldObjectPosition);

            // Rotate the held object to face the same direction as the player
            Quaternion heldObjectRotation = Quaternion.LookRotation(transform.forward);
            _heldDroppable.rigidbody.MoveRotation(heldObjectRotation);
        }
    }
    public void OnPickup(InputValue value)
    {
            TryPickup();
        
    }

    public void OnThrow(InputValue value)
    {
        ThrowObject();
    }

    private void TryPickup()
    {
        if (_heldDroppable == null)
        {
            // Perform a raycast to check if there's an object within pickup distance
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, _pickupDistance) &&
                hit.collider.TryGetComponent(out DroppableComponent droppable))
            {
                _heldDroppable = droppable;
            }
        }
        else
        {
            // Throw the held object in the direction the player is facing
            DropObject();
        }
    }

    private void ThrowObject()
    {
        if (_heldDroppable == null) return;
            _heldDroppable.SetPickedUp(false);
        _heldDroppable.rigidbody.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
    }
    private void DropObject()
    {
        if (_heldDroppable == null) return;
        _heldDroppable.SetPickedUp(false);
    }
}

