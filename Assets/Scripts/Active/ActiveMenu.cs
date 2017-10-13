using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenu : MonoBehaviour
{
    [SerializeField]
    public FPS.Active.ActivatableMenu Menu;

    public void OnActivate()
    {
        Menu.OnActivate();
    }
}
