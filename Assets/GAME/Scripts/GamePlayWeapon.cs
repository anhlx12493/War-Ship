using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayWeapon : GamePlayObject
{
    GamePlayShip _ship;
    public GamePlayShip ship
    {
        get
        {
            if (!_ship)
            {
                _ship = GetComponentInParent<GamePlayShip>();
            }
            return _ship;
        }
    }

    int _amount, _fullAmount;
    public int amount
    {
        get
        {
            return _amount;
        }
    }

    public int fullAmount
    {
        get
        {
            return _fullAmount;
        }
    }

    public bool IsEmpty()
    {
        return _amount <= 0;
    }

    public bool UserAmount()
    {
        if (IsEmpty())
        {
            return false;
        }
        _amount--;
        return true;
    }

    public void AddAmount(int value)
    {
        if (value <= 0)
        {
            return;
        }
        _amount += value;
        if(_amount > _fullAmount)
        {
            _amount = fullAmount;
        }
    }

    public void RefreshAmount()
    {
        _amount = fullAmount;
    }

    protected void SetFullAmount(int value)
    {
        if(value < 0)
        {
            value = 0;
        }
        _fullAmount = value;
    }

    public void UpgradeFullAmount(int upgradeValue)
    {
        if(upgradeValue <= 0)
        {
            return;
        }
        _fullAmount += upgradeValue;
    }

    public void Fire()
    {
        if (UserAmount())
        {
            OnFire();
        }
    }

    public abstract void OnFire();
}
