using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class GamePlayCannon : GamePlayWeapon
{
    NetworkVariableQuaternion networkVariableRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public override void NetworkStart()
    {
        SetFullAmount(int.MaxValue);
        RefreshAmount();
    }

    private void LateUpdate()
    {
        if (IsOwner)
        {
            Debug.DrawLine(transform.position, GamePlayAim.singleton.transform.position);
            if (IsServer)
            {
                networkVariableRotation.Value = Quaternion.FromToRotation(Vector3.forward, GamePlayAim.singleton.transform.position - transform.position);
            }
            else
            {
                UpdateRotationServerRpc(Quaternion.FromToRotation(Vector3.forward, GamePlayAim.singleton.transform.position - transform.position));
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, networkVariableRotation.Value, 0.1f);
    }
    public override void OnFire()
    {
        if (IsOwner)
        {
            SpawBulletServerRpc();
        }
    }

    [ServerRpc]
    void SpawBulletServerRpc()
    {
        GamePlayCannonBullet gamePlayBullet = GamePlayCannonBullet.Spawn(ship.player);
        gamePlayBullet.SetPosition(transform.position);
        gamePlayBullet.transform.rotation = transform.rotation;
        gamePlayBullet.transform.Translate(Vector3.forward * 10f);
        gamePlayBullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }

    [ServerRpc]
    void UpdateRotationServerRpc(Quaternion rotation)
    {
        networkVariableRotation.Value = rotation;
    }
}
