using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public abstract class GamePlayWarMachine : GamePlayObjectPhysic, ObserverHealth
{
    NetworkVariableInt networkVariableHealth = new NetworkVariableInt(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    GamePlayWeapon _weapon;
    GamePlayHealth _health;

    public GamePlayWeapon weapon
    {
        get
        {
            if (!_weapon)
            {
                _weapon = GetComponentInChildren<GamePlayWeapon>();
            }
            return _weapon;
        }
    }

    public GamePlayHealth health
    {
        get
        {
            return _health;
        }
    }

    Player _player;
    public Player player
    {
        get
        {
            return _player;
        }
    }

    public void Ative()
    {
        _health = new GamePlayHealth(this);
        _health.SetFullHealth(10);
        _health.Refresh();
        CanvasGamePlay.InstantiateHealthBar(transform, new Vector3(0, 20, 0), health);
        networkVariableHealth.Value = _health.health;
        if (IsClient)
        {
            StartCoroutine(CoroutineCheckHealth());
        }
        else
        {
            UpdateHealthClientRpc();
        }
    }

    public void SetPlayer(Player player)
    {
        if (player == null)
        {
            return;
        }
        _player = player;
    }

    public void OnHealthEmty()
    {
        Dead();
    }

    public void OnHealthChange()
    {

    }

    protected abstract void Dead();

    public void BeHit(int damge)
    {
        _health.BeHit(damge);
        networkVariableHealth.Value = _health.health;
        UpdateHealthClientRpc();
    }

    [ClientRpc]
    void UpdateHealthClientRpc()
    {
        _health.SetHealth(networkVariableHealth.Value);
    }

    IEnumerator CoroutineCheckHealth()
    {
        while (true)
        {
            if (_health.health != networkVariableHealth.Value)
            {
                _health.SetHealth(networkVariableHealth.Value);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
