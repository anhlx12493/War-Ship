using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayHealthBar : DisplayRatioBar, ObserverHealth
{
    GamePlayHealth health;
    Transform attachTransform;
    Vector3 positionAttach;

    public void Active(Transform attachTransform, Vector3 positionAttach, GamePlayHealth health)
    {
        this.attachTransform = attachTransform;
        this.positionAttach = positionAttach;
        this.health = health;
        health.ResignObserver(this);
        ratio = health.ratio;
    }

    public void OnHealthChange()
    {
        ratio = health.ratio;
    }

    public void OnHealthEmty()
    {
    }

    private void LateUpdate()
    {
        if (!attachTransform) Destroy(gameObject);
        transform.position = Camera.main.WorldToScreenPoint(attachTransform.position + positionAttach);
    }
}