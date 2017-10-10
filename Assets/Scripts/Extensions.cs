using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FPS
{
    public static class Extensions
    {
        public static Vector3 ToVector(this float f)
        {
            return new Vector3(f, f, f);
        }
    }
}
