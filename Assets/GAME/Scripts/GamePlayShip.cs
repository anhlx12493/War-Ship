using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;


public interface ObserverShipDead
{
    public void OnShipDead(GamePlayShip ship);
}

public interface SubjectShipDead
{
    public void ResignObserverShipDead(ObserverShipDead observer);
    public void RemoveObserverShipDead(ObserverShipDead observer);
    public void NotifyObserversShipDead();
}

public interface BehaviorShipAttack
{
    public void Attact();

    public void Display(ShowFeild screen);
}

public class ShipAttackByCannon : BehaviorShipAttack
{
    public int numberBullets;

    public void Attact()
    {

    }

    public void Display(ShowFeild screen)
    {

    }
}

public abstract class BehaviorShip
{
    protected GamePlayShip ship;

    public void SetShip(GamePlayShip ship)
    {
        this.ship = ship;
    }
}

public class BehaviorShipBasic : BehaviorShip
{
    ControlPadBasic controlPad;

    //public
    public float forceMove = 100, forceTurn = 100;

    public BehaviorShipBasic(GamePlayShip ship)
    {
        SetShip(ship);
        if (ControlPad.singleton == null)
        {
            controlPad = new ControlPadBasic(ship);
        }
        else
        {
            ControlPad.singleton.Destroy();
            controlPad = new ControlPadBasic(ship);
        }
        coroutineControl = ship.StartCoroutine(CoroutineControl());
    }

    Coroutine coroutineControl;
    IEnumerator CoroutineControl()
    {
        while (true)
        {
            CheckIfIsBothUpAndDownSetThemToFalse();
            CheckIfIsBothLeftAndRightSetThemToFalse();
            ControlUp();
            ControlDown();
            ControlLeft();
            ControlRight();
            ControlFire();
            yield return null;
        }
    }

    void CheckIfIsBothUpAndDownSetThemToFalse()
    {
        if (controlPad.isUp == true && controlPad.isDown == true)
        {
            controlPad.isUp = controlPad.isDown = false;
        }
    }

    void CheckIfIsBothLeftAndRightSetThemToFalse()
    {
        if (controlPad.isUp == true && controlPad.isDown == true)
        {
            controlPad.isUp = controlPad.isDown = false;
        }
    }

    void ControlUp()
    {
        if (controlPad.isUp)
        {
            if (coroutineMoveUp == null)
            {
                coroutineMoveUp = ship.StartCoroutine(CoroutineMoveUp());
            }
        }
        else
        {
            if (coroutineMoveUp != null)
            {
                ship.StopCoroutine(coroutineMoveUp);
                coroutineMoveUp = null;
            }
        }
    }

    void ControlDown()
    {
        if (controlPad.isDown)
        {
            if (coroutineMoveDown == null)
            {
                coroutineMoveDown = ship.StartCoroutine(CoroutineMoveDown());
            }
        }
        else
        {
            if (coroutineMoveDown != null)
            {
                ship.StopCoroutine(coroutineMoveDown);
                coroutineMoveDown = null;
            }
        }
    }

    void ControlLeft()
    {
        if (controlPad.isLeft)
        {
            if (coroutineTurnLeft == null)
            {
                coroutineTurnLeft = ship.StartCoroutine(CoroutineTurnLeft());
            }
        }
        else
        {
            if (coroutineTurnLeft != null)
            {
                ship.StopCoroutine(coroutineTurnLeft);
                coroutineTurnLeft = null;
            }
        }
    }

    void ControlRight()
    {
        if (controlPad.isRight)
        {
            if (coroutineTurnRight == null)
            {
                coroutineTurnRight = ship.StartCoroutine(CoroutineTurnRight());
            }
        }
        else
        {
            if (coroutineTurnRight != null)
            {
                ship.StopCoroutine(coroutineTurnRight);
                coroutineTurnRight = null;
            }
        }
    }

    void ControlFire()
    {
        if (controlPad.isFire)
        {
            ship.weapon.Fire();
        }
    }

    Coroutine coroutineMoveUp;
    IEnumerator CoroutineMoveUp()
    {
        while (true)
        {
            ship.rigid.AddForce(ship.transform.forward * forceMove);
            yield return new WaitForFixedUpdate();
        }
    }

    Coroutine coroutineMoveDown;
    IEnumerator CoroutineMoveDown()
    {
        while (true)
        {
            ship.rigid.AddForce(-ship.transform.forward * forceMove);
            yield return new WaitForFixedUpdate();
        }
    }

    Coroutine coroutineTurnLeft;
    IEnumerator CoroutineTurnLeft()
    {
        Vector3 torque = new Vector3(0, -forceTurn, 0);
        while (true)
        {
            ship.rigid.AddTorque(torque);
            yield return new WaitForFixedUpdate();
        }
    }

    Coroutine coroutineTurnRight;
    IEnumerator CoroutineTurnRight()
    {
        Vector3 torque = new Vector3(0, forceTurn, 0);
        while (true)
        {
            ship.rigid.AddTorque(torque);
            yield return new WaitForFixedUpdate();
        }
    }
}



public abstract class GamePlayShip : GamePlayWarMachine, SubjectShipDead
{
    //show in inspector


    //private
    BehaviorShipAttack _behaviorAttack;
    BehaviorShip _behavior;
    BehaviorFloat _behaviorFloat;
    CameraFollow cameraFollow;
    List<ObserverShipDead> observerShipDeads = new List<ObserverShipDead>();

    //network value
    NetworkVariableVector3 networkVariablePosition = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.OwnerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    NetworkVariableQuaternion networkVariableRotation = new NetworkVariableQuaternion(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.OwnerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    //public
    public BehaviorShipAttack behaviorAttack
    {
        get
        {
            return _behaviorAttack;
        }
    }
    public BehaviorShip behavior
    {
        get
        {
            return _behavior;
        }
    }
    public BehaviorFloat behaviorFloat
    {
        get
        {
            return _behaviorFloat;
        }
    }

    protected void Active()
    {
        Ative();
        coroutineUpdateTransform = StartCoroutine(CoroutineUpdateTransform());
    }

    protected void SetFloat()
    {
        _behaviorFloat = new BehaviorFloat(this);
        _behaviorFloat.Float();
    }
    protected void SetAttack(BehaviorShipAttack attack)
    {
        _behaviorAttack = attack;
    }
    protected void SetBehavior(BehaviorShip behavior)
    {
        _behavior = behavior;
    }
    protected void SetCameraFollowThis()
    {
        //if (IsOwner)
        //{
        //    if (CameraFollow.singleton != null)
        //        CameraFollow.singleton.Destroy();
        //    cameraFollow = new CameraFollowPosition(this);
        //}
    }

    public void ResignObserverShipDead(ObserverShipDead observer)
    {
        if(!observerShipDeads.Contains(observer))
            observerShipDeads.Add(observer);
    }

    public void RemoveObserverShipDead(ObserverShipDead observer)
    {
        observerShipDeads.Remove(observer);
    }

    public void NotifyObserversShipDead()
    {
        foreach(ObserverShipDead observerShipDead in observerShipDeads)
        {
            observerShipDead.OnShipDead(this);
        }
    }

    Coroutine coroutineUpdateTransform;
    IEnumerator CoroutineUpdateTransform()
    {
        while (true)
        {
            if (IsOwner)
            {
                if (IsServer)
                {
                    networkVariablePosition.Value = transform.position;
                    networkVariableRotation.Value = transform.rotation;
                }
                else
                {
                    UpdateTransformServerRpc(transform.position,transform.rotation);
                }
            }
            transform.position = Vector3.Lerp(transform.position, networkVariablePosition.Value, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkVariableRotation.Value, 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }

    //ServerRpc
    [ServerRpc]
    void UpdateTransformServerRpc(Vector3 position,Quaternion rotation)
    {
        networkVariablePosition.Value = position;
        networkVariableRotation.Value = rotation;
    }

    protected override void Dead()
    {
        _behaviorFloat.Sink();
        foreach(ObserverShipDead observer in observerShipDeads)
        {
            observer.OnShipDead(this);
        }
    }
}
