using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private List<PickableObject> _nearPickableObjects = new List<PickableObject>();
    [SerializeField] private PickableObject _closectPickableObject;

    public Action<IPickable, EHandType> OnPickedUpObject;

    private void Awake()
    {
        // InputService.OnHandPickDropButtonDown += PickUpClosestObject;
    }

    public void PickUpClosestObject(EHandType hand)
    {
        if (_closectPickableObject == null) return;

        RemoveObject(_closectPickableObject);
        _closectPickableObject.PickUp(gameObject);
        OnPickedUpObject?.Invoke(_closectPickableObject.pickable, hand);

        _closectPickableObject = null;
    }

    private void LateUpdate()
    {
        SelectClosestPickableObject();
    }

    void SelectClosestPickableObject()
    {
        if (_nearPickableObjects.Count == 0)
        {
            _closectPickableObject = null;
            return;
        }
        if (_nearPickableObjects.Count == 1)
        {
            _closectPickableObject = _nearPickableObjects[0];
            return;
        }

        float minDistance = float.MaxValue;

        for (int i = 0; i < _nearPickableObjects.Count; i++)
        {
            var pickableObject = _nearPickableObjects[i];
            if (pickableObject == null) continue;

            pickableObject.HideOutline();

            float dist = Vector3.Distance(transform.position, pickableObject.transform.position);
            if (dist < minDistance)
            {
                _closectPickableObject = pickableObject;
                minDistance = dist;
            }
        }

        if (_closectPickableObject != null)
            _closectPickableObject.ShowOutline();
    }

    public void AddObject(PickableObject pickableObject)
    {
        if (_nearPickableObjects.Contains(pickableObject)) return;

        _nearPickableObjects.Add(pickableObject);
    }

    public void RemoveObject(PickableObject pickableObject)
    {
        if (!_nearPickableObjects.Contains(pickableObject)) return;

        pickableObject.HideOutline();
        _nearPickableObjects.Remove(pickableObject);
    }

    private void OnDestroy()
    {
        // InputService.OnHandPickDropButtonDown -= PickUpClosestObject;
    }
}
