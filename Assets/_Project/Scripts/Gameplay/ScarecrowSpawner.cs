using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScarecrowSpawner : MonoBehaviour
{
    [SerializeField] private Scarecrow _scarecrowPrefab;
    [SerializeField] private Transform _spawnPoint;

    private Scarecrow _currentScareCrow;

    Tween _destroyScareCrowTween;

    private void Awake()
    {
        InputService.SpawnScarecrow += SpawnScarecrow;


    }

    private void Start()
    {
        SpawnScarecrow();
    }

    void SpawnScarecrow()
    {
        DestroyLiveScareCrow();
        _currentScareCrow = CreateScareCrow(_spawnPoint);
    }

    Scarecrow CreateScareCrow(Transform spawnPoint)
    {
        var createdScarecrow = Instantiate(_scarecrowPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

        createdScarecrow.transform.localScale = Vector3.zero;
        createdScarecrow.transform.DOScale(Vector3.one, 0.3f);

        GlobalEvents.OnEnemySpawned?.Invoke(createdScarecrow);

        return createdScarecrow;
    }

    void DestroyLiveScareCrow()
    {
        if (_currentScareCrow == null) return;
        if (_destroyScareCrowTween != null) return;

        var scarecrowToDestroy = _currentScareCrow;
        _currentScareCrow = null;

        _destroyScareCrowTween = scarecrowToDestroy.transform.DOScale(Vector3.zero, 0.15f).OnComplete(() =>
        {
            Destroy(scarecrowToDestroy.gameObject);
            _destroyScareCrowTween.Kill();
            _destroyScareCrowTween = null;
        });
    }

    private void OnDestroy()
    {
        InputService.SpawnScarecrow -= SpawnScarecrow;
    }

    private void OnDrawGizmos()
    {
        if (_spawnPoint != null && _scarecrowPrefab != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawMesh(_scarecrowPrefab.GetComponentInChildren<MeshFilter>().sharedMesh,
                -1,
                _spawnPoint.position,
                _spawnPoint.rotation);

            Gizmos.DrawSphere(_spawnPoint.position, 0.3f);
        }
    }
}
