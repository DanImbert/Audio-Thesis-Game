using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] private float _pickupDistance = 3f;
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private LayerMask _pickupLayer;

    private GameObject _heldObject;
    private Rigidbody _heldObjectRigidbody;
    private Transform _heldObjectInitialParent;
    private InputAction _pickupAction;

    private void OnEnable()
    {
        _pickupAction.Enable();
    }

    private void OnDisable()
    {
        _pickupAction.Disable();
    }

    private void Awake()
    {
        _pickupAction = new InputAction("Pickup", InputActionType.Button, "<Keyboard>/e");
        _pickupAction.performed += ctx => Pickup();
    }

    private void FixedUpdate()
    {
        if (_heldObject != null)
        {
            // Move the held object in front of the player
            Vector3 heldObjectPosition = transform.position + transform.forward * _pickupDistance;
            _heldObjectRigidbody.MovePosition(heldObjectPosition);

            // Rotate the held object to face the same direction as the player
            Quaternion heldObjectRotation = Quaternion.LookRotation(transform.forward);
            _heldObjectRigidbody.MoveRotation(heldObjectRotation);
        }
    }

    private void Pickup()
    {
        if (_heldObject == null)
        {
            // Perform a raycast to check if there's an object within pickup distance
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _pickupDistance, _pickupLayer))
            {
                _heldObject = hit.collider.gameObject;
                _heldObjectRigidbody = _heldObject.GetComponent<Rigidbody>();
                _heldObjectInitialParent = _heldObject.transform.parent;
                _heldObjectRigidbody.useGravity = false;
                _heldObjectRigidbody.isKinematic = true;
                _heldObject.transform.SetParent(transform);
            }
        }
        else
        {
            // Throw the held object in the direction the player is facing
            _heldObjectRigidbody.useGravity = true;
            _heldObjectRigidbody.isKinematic = false;
            _heldObjectRigidbody.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
            _heldObject.transform.SetParent(_heldObjectInitialParent);
            _heldObject = null;
            _heldObjectRigidbody = null;
        }
    }
}

