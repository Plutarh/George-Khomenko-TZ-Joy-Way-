using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEvents
{
    public static Action<Pawn> OnEnemySpawned;
    public static Action<string> OnWeaponDestroyed;
    public static Action SpawnScarecrow;
    public static Action OnRestartBtnDown;

}
