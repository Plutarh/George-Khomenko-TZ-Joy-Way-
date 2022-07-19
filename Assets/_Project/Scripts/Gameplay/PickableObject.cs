using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public IPickable pickable;

    [SerializeField] private float _pickRadius = 2;
    [SerializeField] private Outlinable _outlineable;

    SphereCollider _sphereCollider;
    Rigidbody _body;
    private FloatingObject _floatingObject;

    private void Awake()
    {
        pickable = transform.root.GetComponent<IPickable>();

        _outlineable = transform.root.gameObject.GetComponent<Outlinable>();

        if (_floatingObject == null)
            _floatingObject = gameObject.AddComponent<FloatingObject>();

        SetupRigidbody();
        SetupTriggerCollider();
        HideOutline();
    }

    void SetupRigidbody()
    {
        _body = transform.root.GetComponent<Rigidbody>();

        if (_body == null)
            _body = transform.root.gameObject.AddComponent<Rigidbody>();

        _body.isKinematic = true;
    }

    void SetupTriggerCollider()
    {
        _sphereCollider = transform.root.gameObject.AddComponent<SphereCollider>();
        _sphereCollider.radius = _pickRadius;
        _sphereCollider.isTrigger = true;
    }

    public void ShowOutline()
    {
        _outlineable.enabled = true;
    }

    public void HideOutline()
    {
        _outlineable.enabled = false;
    }


    void RemoveComponents()
    {
        HideOutline();
        // Destroy(_outlineable);
        Destroy(_sphereCollider);
        Destroy(_body);
        _floatingObject.StopFloating();
        Destroy(_floatingObject);
        Destroy(this);
    }

    public void PickUp(GameObject owner)
    {
        RemoveComponents();
        pickable.PickUp(owner);
    }

    private void OnTriggerEnter(Collider other)
    {
        var objectPicker = other.transform.root.GetComponent<ObjectPicker>();

        if (objectPicker == null) return;

        objectPicker.AddObject(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var objectPicker = other.transform.root.GetComponent<ObjectPicker>();

        if (objectPicker == null) return;

        objectPicker.RemoveObject(this);
    }
}
