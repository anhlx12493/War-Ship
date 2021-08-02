using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    //Prefab
    public static readonly string prefabGamePlayAim = "PrefabsGamePlay/GamePlayAim";
    public static readonly string prefabGamePlayBulletCannon = "PrefabsGamePlay/GamePlayBulletCannon";
    public static readonly string prefabGamePlayShipCannon = "PrefabsGamePlay/GamePlayShipCannon";
    public static readonly string prefabCanvasGamePlay = "PrefabsGamePlay/CanvasGamePlay";
    public static readonly string prefabGamePlayHealthBar = "PrefabsGamePlay/GamePlayHealthBar";

    public static GameObject InstantiatePrefab(string address)
    {
        return Instantiate(Resources.Load(address)) as GameObject;
    }

    public static GameObject InstantiatePrefab(string address,Transform parrent)
    {
        return Instantiate(Resources.Load(address), parrent) as GameObject;
    }
}
