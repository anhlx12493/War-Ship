using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class TestMove : NetworkBehaviour
{
    NetworkVariableVector3 position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public override void NetworkStart()
    {
        Move();
        if (IsOwner)
            Debug.Log("AAAAAAA");
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, 0.2f) * 2f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -0.2f) * 2f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-0.2f, 0, 0f) * 2f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.2f, 0, 0) * 2f;
        }
        if (NetworkManager.Singleton.IsServer)
        {
            position.Value = transform.position;
        }
        else
        {
            MoveServerRpc(transform.position);
        }
    }

    [ServerRpc]
    void MoveServerRpc(Vector3 value)
    {
        position.Value = value;
    }

    void Update()
    {
        if (IsOwner) 
        {
            Move();
        }
        transform.position = Vector3.Lerp(transform.position, position.Value, 0.1f);
    }
}
