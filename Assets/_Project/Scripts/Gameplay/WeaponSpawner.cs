using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private List<WeaponSpawnInfo> _weaponSpawnInfos = new List<WeaponSpawnInfo>();

    private void OnValidate()
    {
        foreach (var wsi in _weaponSpawnInfos)
        {
            if (wsi.weaponPrefab == null) continue;

            wsi.weaponName = wsi.weaponPrefab.name;
        }
    }

    private void Awake()
    {
        SpawnWeapons();
    }

    void RecreateWeapon(string weaponName)
    {
        var foundedWeaponSpawnInfo = _weaponSpawnInfos.FirstOrDefault(wsi => wsi.weaponName == weaponName);

        if (foundedWeaponSpawnInfo == null)
        {
            Debug.LogError($"Cannot recreate weapon : {weaponName}");
            return;
        }

        foundedWeaponSpawnInfo.CurrentLiveCount--;
    }

    void SpawnWeapons()
    {
        _weaponSpawnInfos.ForEach(wsi => SpawnWeapon(wsi));
    }

    void SpawnWeapon(WeaponSpawnInfo wsi)
    {
        while (wsi.CurrentLiveCount < wsi.maxLiveCount)
        {
            Vector3 spawnPosition = Vector3.zero;

            if (wsi.CurrentLiveCount >= wsi.spawnPoints.Count)
                spawnPosition = wsi.spawnPoints[0].position;

            var createdWeapon = Instantiate(wsi.weaponPrefab, wsi.spawnPoints[wsi.CurrentLiveCount].position + Vector3.up, Quaternion.identity);

            createdWeapon.Drop();

            wsi.CurrentLiveCount++;
        }
    }

    void Start()
    {

    }


    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var wsi in _weaponSpawnInfos)
        {
            if (wsi.weaponPrefab == null) continue;

            foreach (var spawnPoint in wsi.spawnPoints)
            {
                Gizmos.DrawCube(spawnPoint.position, Vector3.one * 0.5f);
            }

        }
    }
}

[System.Serializable]
public class WeaponSpawnInfo
{
    [HideInInspector] public string weaponName;
    public int CurrentLiveCount
    {
        get
        {
            return _currentLiveCount;
        }
        set
        {
            if (value > maxLiveCount)
                _currentLiveCount = maxLiveCount;
            else
            {
                _currentLiveCount = value;
                if (_currentLiveCount < 0)
                    _currentLiveCount = 0;
            }
        }

    }
    public Weapon weaponPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public int maxLiveCount = 2;
    private int _currentLiveCount;
}
