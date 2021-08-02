using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;

public abstract class BulletMovement : GameBehavior
{
    public abstract void FixedUpdate();
}

public class BulletMoveStraight : BulletMovement
{
    Vector3 worldSpeed = Vector3.forward;

    public BulletMoveStraight(MonoBehaviour subject)
    {
        SetSubject(subject);
    }

    public override void FixedUpdate()
    {
        subject.transform.Translate(worldSpeed);
    }

    public void SetSpeed(float speed)
    {
        worldSpeed = speed * Vector3.forward;
    }
}

public abstract class GamePlayBullet : GamePlayObject
{
    BulletMovement movement;

    protected int damge = 1;

    Player _player;
    public Player player
    {
        get
        {
            return _player;
        }
    }

    float _lifeTime = 3;
    public float lifeTime
    {
        get
        {
            return _lifeTime;
        }
    }

    public void Active()
    {
        coroutineUpdateBehaviors = StartCoroutine(CoroutineUpdateBehaviors());
    }

    public void SetMovement(BulletMovement movement)
    {
        this.movement = movement;
    }

    public void TrySetPlayer(Player player)
    {
        if(_player == null)
        {
            _player = player;
        }
    }

    public void SetLifeTime(float value)
    {
        if (value <= 0)
            return;
        _lifeTime = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            GamePlayWarMachine gamePlayWarMachine = other.GetComponent<GamePlayWarMachine>();
            if (gamePlayWarMachine)
            {
                if (player != gamePlayWarMachine.player)
                {
                    HitWarMachine(gamePlayWarMachine);
                }
            }
        }
    }

    void HitWarMachine(GamePlayWarMachine machine)
    {
        machine.BeHit(damge);
        Destroy(gameObject);
    }

    Coroutine coroutineUpdateBehaviors;
    IEnumerator CoroutineUpdateBehaviors()
    {
        float startTime = Time.time;
        while (Time.time - startTime < lifeTime)
        {
            movement.FixedUpdate();
            yield return new WaitForFixedUpdate();
        }
        if (IsOwner)
        {
            DestroyServerRpc();
        }
    }

    [ServerRpc]
    void DestroyServerRpc()
    {
        Destroy(gameObject);
    }
}
