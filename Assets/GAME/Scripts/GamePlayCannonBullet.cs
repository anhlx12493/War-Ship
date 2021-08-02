using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class GamePlayCannonBullet : GamePlayBullet
{
    NetworkVariableVector3 networkVariablePosition = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });


    public override void NetworkStart()
    {
        BulletMoveStraight bulletMoveStraight = new BulletMoveStraight(this);
        bulletMoveStraight.SetSpeed(3);
        SetMovement(bulletMoveStraight);
        Active();
        UpdatMove();
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        if (IsOwner)
        {
            if (IsServer)
            {
                networkVariablePosition.Value = transform.position;
            }
            else
            {
                UpdateMoveServerRpc(transform.position);
            }
        }
    }

    private void Update()
    {
        UpdatMove();
    }

    void UpdatMove()
    {
        if (IsOwner)
        {
            if (IsServer)
            {
                networkVariablePosition.Value = transform.position;
            }
            else
            {
                UpdateMoveServerRpc(transform.position);
            }
        }
    }

    [ServerRpc]
    void UpdateMoveServerRpc(Vector3 position)
    {
        networkVariablePosition.Value = position;
    }

    public static GamePlayCannonBullet Spawn(Player byPlayer)
    {
        GamePlayCannonBullet gamePlayBullet = PrefabManager.InstantiatePrefab(PrefabManager.prefabGamePlayBulletCannon).GetComponent<GamePlayCannonBullet>();
        gamePlayBullet.TrySetPlayer(byPlayer);
        return gamePlayBullet;
    }
}
