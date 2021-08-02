using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayAim : MonoBehaviour
{
    private static GamePlayAim _singleton;
    public static GamePlayAim singleton
    {
        get
        {
            Create();
            return _singleton;
        }
    }

    public static void Create()
    {
        if (!_singleton)
        {
            _singleton = PrefabManager.InstantiatePrefab(PrefabManager.prefabGamePlayAim).GetComponent<GamePlayAim>();
            DontDestroyOnLoad(_singleton.gameObject);
        }
    }

    [SerializeField] LayerMask layerRay;
    Ray ray;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (layerRay > 0)
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerRay))
            {
                transform.position = raycastHit.point;
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                transform.position = raycastHit.point;
            }
        }
    }

    public static void Destroy()
    {
        if(_singleton)
            Destroy(_singleton.gameObject);
    }
}
