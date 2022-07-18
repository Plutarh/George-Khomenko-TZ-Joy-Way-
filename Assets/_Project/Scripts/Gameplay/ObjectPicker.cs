using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    [SerializeField] private List<PickableObject> _nearPickableObjects = new List<PickableObject>();
    [SerializeField] private PickableObject _closectPickableObject;

    private void LateUpdate()
    {
        SelectClosestPickableObject();
    }

    void SelectClosestPickableObject()
    {
        if (_nearPickableObjects.Count == 0) return;
        if (_nearPickableObjects.Count == 1)
        {
            _closectPickableObject = _nearPickableObjects[0];
            return;
        }

        float minDistance = float.MaxValue;

        for (int i = 0; i < _nearPickableObjects.Count; i++)
        {
            var pickableObject = _nearPickableObjects[i];

            float dist = Vector3.Distance(transform.position, pickableObject.transform.position);
            if (dist < minDistance)
            {
                _closectPickableObject = pickableObject;
                minDistance = dist;
            }
        }
    }

    public void AddObject(PickableObject pickableObject)
    {
        if (_nearPickableObjects.Contains(pickableObject)) return;

        _nearPickableObjects.Add(pickableObject);
    }

    public void RemoveObject(PickableObject pickableObject)
    {
        if (!_nearPickableObjects.Contains(pickableObject)) return;

        _nearPickableObjects.Remove(pickableObject);
    }
}
