using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

//namespace Math
//{
    public class Math
    {
        /// <summary>
        /// Normalizes float 'f' to 0-1 by bounds
        /// </summary>
        /// <param name="f">float to normalize</param>
        /// <param name="lower">lower bounds (inclusive)</param>
        /// <param name="upper">upper bounds (inclusive)</param>
        /// <returns></returns>
        public static float Normalize(float f, float lower, float upper)
        {
            return (f - lower) / (upper - lower);
        }

        /// <summary>
        /// Normalizes float 'f' to range normalizedLower-normalizedUpper by bounds
        /// </summary>
        /// <param name="f">float to normalize</param>
        /// <param name="lower">lower bounds (inclusive)</param>
        /// <param name="upper">upper bounds (inclusive)</param>
        /// <param name="normalizedLower">the new lower bounds (inclusive)</param>
        /// <param name="normalizedUpper">the new upper bounds (inclusive)</param>
        /// <returns></returns>
        public static float NormalizeRange(float f, float lower, float upper, float normalizedLower, float normalizedUpper)
        {
            float a = Normalize(f, lower, upper);

            return (a * (normalizedUpper - normalizedLower)) + normalizedLower;
        }

        /// <summary>
        /// Normalizes float 'f' to range normalizedLower-normalizedUpper by bounds and then inverses the result
        /// </summary>
        /// <param name="f">float to normalize</param>
        /// <param name="lower">lower bounds (inclusive)</param>
        /// <param name="upper">upper bounds (inclusive)</param>
        /// <param name="normalizedLower">the new lower bounds (inclusive)</param>
        /// <param name="normalizedUpper">the new upper bounds (inclusive)</param>
        /// <returns></returns>
        public static float InverseNormalizeRange(float f, float lower, float upper, float normalizedLower, float normalizedUpper)
        {
        if (f > upper)
            f = upper;
        else if (f < lower)
            f = lower;

            float a = Normalize(f, lower, upper);

            return (normalizedUpper - ((a * (normalizedUpper - normalizedLower)) + normalizedLower)) + normalizedLower;
        }

        /// <summary>
        /// Returns the difference between two floats regardless of order.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AbsDifference(float a, float b)
        {
            if (a > b)
                return a - b;
            else return b - a;
        }

        /// <summary>
        /// Checks to see if a specified direction is left or right of another.
        /// </summary>
        /// <param name="fwd">The centerline direction</param>
        /// <param name="targetDir">The direction to check</param>
        /// <param name="up">Up direction (usually Vector3.up)</param>
        /// <returns>Negative if angle is to the left, positive if to the right, and 0 if straight.</returns>
        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            return dir;
        }

        /// <summary>
        /// Checks to see if a specified direction is left or right of another in 2D space.
        /// </summary>
        /// <param name="A">The centerline direction</param>
        /// <param name="B">The direction to check</param>
        /// <returns>Negative if angle is to the left, positive if to the right, and 0 if straight.</returns>
        public static float AngleDir2D(Vector2 A, Vector2 B)
        {
            return -A.x * B.y + A.y * B.x;
        }

        /// <summary>
        /// Gets the normalized direction between two points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Returns the normalized direction vector between two positions.</returns>
        public static Vector3 Direction(Vector3 a, Vector3 b)
        {
            Vector3 heading = b - a;
            return heading / heading.magnitude;
        }

        /// <summary>
        /// Gets the normalized direction between two points, throwing out vertical space.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Returns the normalized direction vector between two positions without the y component.</returns>
        public static Vector3 FlatDirection(Vector3 a, Vector3 b)
        {
            Vector3 heading = new Vector3(b.x, 0, b.z) - new Vector3(a.x, 0, a.z);
            return heading / heading.magnitude;
        }

        /// <summary>
        /// Gets the distance between two points, throwing out vertical space.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Returns the distance between two positions without the y component.</returns>
        public static float FlatDistance(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(new Vector3(a.x, 0, a.z), new Vector3(b.x, 0, b.z));
        }

        /// <summary>
        /// Gets the distance between two points (target, end) only in the direction between start and end. Positive if end is behind target, negative if in front.
        /// </summary>
        /// <param name="fwd"></param>
        /// <param name="point"></param>
        /// <returns>The distance between two points (target, end) only in the direction between start and end. Positive if end is behind target, negative if in front.</returns>
        public static float ForwardDistance(Vector3 start, Vector3 target, Vector3 end)
        {
            float d1 = Vector3.Distance(start, end);
            float d2 = Vector3.Distance(target, end);

            return d2 - d1;
        }

        public static float DegreesToRadians(float r)
        {
            return r * 3.14f / 180f;
        }

        public static float RadiansToDegrees(float r)
        {
            return r * 180f / 3.14f;
        }
    }
//}
