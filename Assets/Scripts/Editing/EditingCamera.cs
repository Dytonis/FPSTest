using cakeslice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Editing
{
    public class EditingCamera : MonoBehaviour
    {
        public Camera OrthoCamera;
        public Editable EditingObject;

        public void Update()
        {
            Vector3 screen = OrthoCamera.ScreenToWorldPoint(Input.mousePosition);

            screen.Set(screen.x, screen.y, OrthoCamera.transform.position.z);

            Debug.DrawRay(screen, OrthoCamera.transform.forward, Color.red, 0.1f);

            EditingObject.Reset();

            RaycastHit h;
            if (Physics.Raycast(screen, OrthoCamera.transform.forward, out h, 100f))
            {
                Editable e = h.transform.parent.parent.GetComponent<Editable>();

                if (e)
                {
                    EditableZone z = e.Zones.Single(x => x.Colliders.Any(y => y == h.collider));

                    foreach(Outline o in z.OutlineRenderers)
                    {
                        o.enabled = true;
                    }
                }
            }
        }
    }
}
