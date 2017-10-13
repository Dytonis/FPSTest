using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.Active;
using System;

public class Workshop : ActivatableMenu
{
    public GameObject WorkshopScreen;
    public PlayerController controller;

    public override void OnActivate()
    {
        WorkshopScreen.gameObject.SetActive(true);
        controller.PlayerCamera.enabled = false;
        controller.gameObject.SetActive(false);
    }

    public override void OnDeactivate()
    {
        WorkshopScreen.gameObject.SetActive(false);
        controller.PlayerCamera.enabled = true;
        controller.gameObject.SetActive(true);
    }
}
