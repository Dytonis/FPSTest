using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Physics
{
    public class BallisticProjectile : MonoBehaviour
    {
        public GameObject VisibleObject;
        public Vector3 Velocity;
        public Vector3 Gravity = new Vector3(0, -9.81f, 0);
        private Camera cameraToUse;
        [Range(0, 1)]
        public float ApplyScale;
        [Range(0, 1)]
        public float Drag;
        public float DistanceAtMin;
        public float ScreenSize = 15;
        public float MinScreenSize;

        public void Start()
        {
            Destroy(gameObject, 5);
        }

        public void SetForwardVelocity(float forward)
        {
            Velocity.Set(Velocity.x, Velocity.y, forward);
        }

        public void SetCamera(Camera c)
        {
            cameraToUse = c;
        }

        // Update is called once per frame
        void Update()
        {
            OnPositionUpdate();
        }

        public virtual void OnPositionUpdate()
        {
            Velocity += Gravity * Time.deltaTime;
            Velocity *= (1 - (Drag * Time.deltaTime));

            RaycastHit hit;

            if (ForwardRaycast(out hit))
            {
                transform.position = hit.point;
                OnHit(hit);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.InverseTransformDirection(new Vector3(-Velocity.x, -Velocity.y, Velocity.z) * Time.deltaTime), Color.red, Time.deltaTime);
                transform.localPosition += (transform.localRotation * Velocity) * Time.deltaTime;
                VisibleObject.transform.localRotation = Quaternion.Euler(transform.forward);
            }

            ScreenScale(ScreenSize, MinScreenSize);
        }

        public virtual void OnHit(RaycastHit hit)
        {

        }

        public virtual bool ForwardRaycast(out RaycastHit hit)
        {
            RaycastHit h;

            bool b = UnityEngine.Physics.Raycast(transform.position, transform.forward, out h, Velocity.magnitude * Time.deltaTime);

            hit = h;

            return b;
        }

        private void ScreenScale(float sizeOnScreen, float minSize)
        {
            float distance = Vector3.Distance(cameraToUse.transform.position, gameObject.transform.position);
            float height = 2.0f * Mathf.Tan(0.5f * ((60 + (cameraToUse.fieldOfView * 3)) / 4) * Mathf.Deg2Rad) * distance;
            float width = height * Screen.width / Screen.height;

            float size = 1f / sizeOnScreen;
            float minsize = 1f / minSize;

            ApplyScale = Math.InverseNormalizeRange(distance, 100, DistanceAtMin, 0, 1);

            float scale = ((width / size) * ApplyScale) + (width / minsize) * (1 - ApplyScale);
            if (float.IsNaN(scale))
                return;

            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            VisibleObject.GetComponent<TrailRenderer>().widthMultiplier = (transform.localScale.magnitude / 2.5f) * 1.41f;
        }
    }
}
