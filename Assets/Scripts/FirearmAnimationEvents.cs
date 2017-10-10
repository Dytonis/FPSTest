using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FPS.Equipable;
using FPS.Physics;
using FPS;

public partial class Firearm : Equipable
{
    public void AE_BoltFinished()
    {
        AnimationController.SetBool("NeedsBoltPull", false);
    }
}
