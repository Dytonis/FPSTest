using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editable : MonoBehaviour
{
    public EditableZone[] Zones;

    public float rotationX;
    public float rotationY;

    public void Reset()
    {
        foreach (EditableZone z in Zones)
        {
            foreach (Outline o in z.OutlineRenderers)
                o.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxisRaw("Mouse X") / 4;
            rotationY += Input.GetAxisRaw("Mouse Y") / 4;

            transform.parent.localRotation = Quaternion.Euler(-rotationY, 0, rotationX);
        }
    }

    private void Start()
    {
        Reset();
    }
}

[System.Serializable]
public struct EditableZone
{
    public string ZoneName;
    public AttachmentType Type;
    public Collider[] Colliders;
    public Outline[] OutlineRenderers;
    public Action InvokedAction;
}

public enum AttachmentType
{
    Rail,
    Barrel,
    Chamber,
    Threaded,
    Magazine,
    Buttpad
}
