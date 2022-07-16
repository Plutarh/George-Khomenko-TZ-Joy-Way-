using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstaller : MonoBehaviour
{

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _playerSpawnPosition;
    [SerializeField] private PlayerCamera _playerCameraPrefab;
    [SerializeField] private InputService _inputService;

    private Player _playerInstance;
    private PlayerCamera _playerCameraInstance;

    private void Awake()
    {
        CreatePlayer();
        CreateCamera();
    }

    void CreatePlayer()
    {
        _playerInstance = Instantiate(_playerPrefab, _playerSpawnPosition.position, _playerSpawnPosition.rotation);
    }

    void CreateCamera()
    {
        _playerCameraInstance = Instantiate(_playerCameraPrefab, _playerInstance.transform.position, Quaternion.identity);
        _playerCameraInstance.Initialize(_playerInstance, _inputService);
    }
}
