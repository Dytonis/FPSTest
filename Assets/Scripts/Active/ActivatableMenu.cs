using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FPS.Active
{
    public abstract class ActivatableMenu : MonoBehaviour
    {
        public abstract void OnActivate();
        public abstract void OnDeactivate();
    }
}
