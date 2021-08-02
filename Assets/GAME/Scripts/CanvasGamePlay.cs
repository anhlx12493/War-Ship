using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGamePlay : MonoBehaviour
{
    static CanvasGamePlay _singleton;
    public static CanvasGamePlay singleton
    {
        get
        {
            if (!_singleton)
            {
                _singleton = PrefabManager.InstantiatePrefab(PrefabManager.prefabCanvasGamePlay).GetComponent<CanvasGamePlay>();
            }
            return _singleton;
        }
    }

    public static void InstantiateHealthBar(Transform attachTransform, Vector3 positionAttach, GamePlayHealth health)
    {
        DisplayHealthBar healthBar = PrefabManager.InstantiatePrefab(PrefabManager.prefabGamePlayHealthBar, singleton.transform).GetComponent<DisplayHealthBar>();
        healthBar.Active(attachTransform, positionAttach, health);
    }
}
