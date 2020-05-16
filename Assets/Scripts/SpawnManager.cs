using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Service
{
    public override void RegisterService()
    {
        ServiceLocator.Instance.Register(this);
    }
}
