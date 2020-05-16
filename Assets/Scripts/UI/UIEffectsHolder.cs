using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectsHolder : Service
{
    public GameObject confettiVFX;
    public GameObject fireworkVFX;
    public GameObject stimulText;
    
    public override void RegisterService()
    {
        ServiceLocator.Instance.Register(this);
    }
}