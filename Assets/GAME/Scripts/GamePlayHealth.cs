using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ObserverHealth
{
    public void OnHealthChange();
    public void OnHealthEmty();
}

public class GamePlayHealth
{
    bool _isDead;
    int _health, _fullHealth;
    List<ObserverHealth> observerHealths = new List<ObserverHealth>();

    public int health
    {
        get
        {
            return _health;
        }
    }

    public int fullHealth
    {
        get
        {
            return _fullHealth;
        }
    }

    public float ratio
    {
        get
        {
            return (float)_health / fullHealth;
        }
    }

    public GamePlayHealth(ObserverHealth observer)
    {
        observerHealths.Add(observer);
    }

    public void ResignObserver(ObserverHealth observer)
    {
        if (!observerHealths.Contains(observer))
        {
            observerHealths.Add(observer);
        }
    }

    public void SetFullHealth(int value)
    {
        if (value < 0) value = 0;
        _fullHealth = value;
    }

    public void Refresh()
    {
        _health = _fullHealth;
        _isDead = false;
    }

    public void BeHit(int damge)
    {
        if (_isDead) return;
        if (damge <= 0)
            return;
        _health -= damge;
        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            foreach (ObserverHealth observer in observerHealths)
                observer.OnHealthEmty();
        }
        foreach (ObserverHealth observer in observerHealths)
            observer.OnHealthChange();
    }

    public void SetHealth(int value)
    {
        if (_isDead) return;
        _health = value;
        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            foreach (ObserverHealth observer in observerHealths)
                observer.OnHealthEmty();
        }
        foreach (ObserverHealth observer in observerHealths)
            observer.OnHealthChange();
    }
}

