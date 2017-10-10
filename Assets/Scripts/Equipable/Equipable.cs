using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS.Equipable
{
    public class Equipable : MonoBehaviour
    {
        protected float CooldownTimer;
        public Animator AnimationController;

        public virtual void Update()
        {
            if (CooldownTimer > 0)
                CooldownTimer -= Time.deltaTime;
            else if (CooldownTimer < 0)
                CooldownTimer = 0;

            GetInput();
        }

        protected virtual void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsPrimary = true;
                Primary();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                IsPrimary = false;
                UpPrimary();
            }
            if(Input.GetMouseButton(0))
            {
                IsPrimary = true;
            }
            else
            {
                IsPrimary = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                IsSecondary = true;
                Secondary();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                IsSecondary = false;
                UpSecondary();
            }
            if (Input.GetMouseButton(1))
            {
                IsSecondary = true;
            }
            else
            {
                IsSecondary = false;
            }
        }
        public virtual void Primary() { }
        public virtual void UpPrimary() { }
        public bool IsPrimary;
        public virtual void Secondary() { }
        public virtual void UpSecondary() { }
        public bool IsSecondary { get; set; }
        public bool Consumable;
        public int ConsumableStack;
    }
}
