using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class HelloWorldPlayer : NetworkBehaviour
{
    NetworkVariableVector3 position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public override void NetworkStart()
    {
        Move();
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                position.Value += new Vector3(0, 0, 0.2f) * 2f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                position.Value += new Vector3(0, 0, -0.2f) * 2f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                position.Value += new Vector3(-0.2f, 0, 0f) * 2f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                position.Value += new Vector3(0.2f, 0, 0) * 2f;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveUpServerRpc();
            }
            if (Input.GetKey(KeyCode.S))
            {
                MoveDownServerRpc();
            }
            if (Input.GetKey(KeyCode.A))
            {
                MoveLeftServerRpc();
            }
            if (Input.GetKey(KeyCode.D))
            {
                MoveRightServerRpc();
            }
        }
    }

    [ServerRpc]
    void MoveUpServerRpc()
    {
        position.Value += new Vector3(0, 0, 0.2f) * 2f;
    }

    [ServerRpc]
    void MoveDownServerRpc()
    {
        position.Value += new Vector3(0, 0, -0.2f) * 2f;
    }

    [ServerRpc]
    void MoveLeftServerRpc()
    {
        position.Value += new Vector3(-0.2f, 0, 0f) * 2f;
    }

    [ServerRpc]
    void MoveRightServerRpc()
    {
        position.Value += new Vector3(0.2f, 0, 0) * 2f;
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-60f, 60f), 1f, Random.Range(-60f, 60f));
    }

    void Update()
    {
        if (IsOwner)
        {
            Move();
        }
        if (IsClient)
            transform.position = Vector3.Lerp(transform.position, position.Value, 0.1f);
    }
}
