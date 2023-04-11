using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableComponent : MonoBehaviour
{
    public Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void SetPickedUp(bool value)
    {
        rigidbody.useGravity = !value;
        rigidbody.isKinematic = value;
    }
}
