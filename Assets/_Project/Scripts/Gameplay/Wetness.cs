using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wetness : MonoBehaviour
{
    [SerializeField] private WetnessScriptableEffect _wetnessEffect;
    private Pawn _pawn;

    private void Awake()
    {
        _pawn = transform.root.gameObject.GetComponent<Pawn>();
    }
}
