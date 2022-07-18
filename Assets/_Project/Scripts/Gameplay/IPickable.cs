using UnityEngine;

public interface IPickable
{
    void PickUp(GameObject owner);
    void Drop();
}

