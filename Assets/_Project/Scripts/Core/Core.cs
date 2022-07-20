using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    private void Awake()
    {
        GlobalEvents.OnRestartBtnDown += RestartLevel;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnDestroy()
    {
        GlobalEvents.OnRestartBtnDown -= RestartLevel;
    }
}
