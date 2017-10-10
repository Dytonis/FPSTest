using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FPS.Equipable;
using FPS.Physics;
using FPS;

public partial class Firearm : Equipable
{
    public Camera PlayerCamera;
    public Transform RightHand;
    public Vector3 ADSPosition;
    public PlayerController controller;
    public FirearmStatsInfo Stats;
    public CameraKickInfo KickInfo;
    public Bullet BulletPrefab;

    private Vector3 RightHandDefault;

    public void Start()
    {
        RightHandDefault = RightHand.localPosition;
    }

    public override void Primary()
    {
        StartCoroutine(Fire());
    }

    public override void Secondary()
    {
        StopCoroutine("ADS");
        StopCoroutine("BackADS");
        StartCoroutine(ADS(3));
        controller.ActualSensitivity = 0.2f;
        controller.ToFOV(35, 70);
    }

    public override void UpSecondary()
    {
        StopCoroutine("ADS");
        StopCoroutine("BackADS");
        StartCoroutine(BackADS(6));
        controller.ActualSensitivity = controller.Sensitivity;
        controller.ToFOV(60, 300);
    }

    public virtual IEnumerator ADS(float speed)
    {
        float distance = 1;

        while(distance > 0.001f && IsSecondary)
        {
            Vector3 n = Vector3.MoveTowards(RightHand.localPosition, ADSPosition, Time.deltaTime * speed);
            distance = Vector3.Distance(RightHand.localPosition, ADSPosition);
            RightHand.localPosition = n;
            yield return new WaitForEndOfFrame();
        }
    }

    public virtual IEnumerator BackADS(float speed)
    {
        float distance = 1;

        while (distance > 0.001f && IsSecondary == false)
        {
            Vector3 n = Vector3.MoveTowards(RightHand.localPosition, RightHandDefault, Time.deltaTime * speed);
            distance = Vector3.Distance(RightHand.localPosition, RightHandDefault);
            RightHand.localPosition = n;
            yield return new WaitForEndOfFrame();
        }
    }

    public virtual IEnumerator Fire()
    {
        Transform muzzle = transform.FindChild("Muzzle");

        if (Stats.Action == FirearmAction.Full)
        {
            while (IsPrimary)
            {
                if (CooldownTimer <= 0)
                    SpawnProjectile(muzzle);

                yield return new WaitForEndOfFrame();
            }
        }
        else if (Stats.Action == FirearmAction.Bolt)
        {
            if (CooldownTimer <= 0)
            {
                SpawnProjectile(muzzle);

                yield return new WaitForSeconds(Stats.BoltWaitTime);

                if (AnimationController)
                    AnimationController.SetBool("NeedsBoltPull", true);
            }
        }

        yield break;
    }

    private bool GetCameraRay(out RaycastHit h)
    {
        RaycastHit hit;

        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * 1000, Color.red, 1f);

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit))
        {
            h = hit;
            return true;
        }
        else
        {
            h = hit;
            return false; 
        }
    }

    private void SpawnProjectile(Transform muzzle)
    {
        if (AnimationController)
            AnimationController.SetTrigger("FireNormal");

        RaycastHit h;
        if (GetCameraRay(out h))
        {
            muzzle.LookAt(h.point, Vector3.up);
        }
        else
        {
            muzzle.transform.localEulerAngles = Vector3.zero;
        }

        if (BulletPrefab != null)
        {
            for (int i = 0; i < BulletPrefab.Flare.Count; i++)
            {
                GameObject flare = Instantiate(BulletPrefab.Flare.Object, muzzle) as GameObject;
                flare.transform.localScale *= BulletPrefab.Flare.Size;
                flare.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 180));
                StartCoroutine(FlareFade(flare, BulletPrefab.Flare.Life));
                Destroy(flare.gameObject, BulletPrefab.Flare.Life);
            }
        }
        BallisticProjectile b = Instantiate(BulletPrefab, muzzle.position, muzzle.rotation) as BallisticProjectile;
        b.SetCamera(PlayerCamera);
        b.SetForwardVelocity(Stats.BulletVelocity);

        //controller.Rotation(Random.Range(-KickInfo.SideKick - KickInfo.SideKickRandomness, KickInfo.SideKick + KickInfo.SideKickRandomness), Random.Range(-KickInfo.VerticalKick - KickInfo.VerticalKickRandomness, -KickInfo.VerticalKick + KickInfo.VerticalKickRandomness));
        StopCoroutine("Kick");
        StartCoroutine(Kick());

        CooldownTimer = (1 / (Stats.RPM / 60));
    }

    public virtual IEnumerator Kick()
    {
        float t = 0;

        //Debug.Log("jerk: " + CameraJerk.transform.localEulerAngles.x)

        controller.EQCameraYJerk();

        float boundsVertical = KickInfo.VerticalMotion - (KickInfo.VerticalMotion * (1 - KickInfo.VerticalMotionRandomness));
        float randomVertical = Random.Range(KickInfo.VerticalMotion - boundsVertical, KickInfo.VerticalMotion + boundsVertical);

        float boundsSide = KickInfo.SideMotion - (KickInfo.SideMotionRandomness * (1 - KickInfo.SideMotionRandomness));
        float randomSide = Random.Range(KickInfo.SideMotion - boundsSide, KickInfo.SideMotion + boundsSide);

        float boundsVerticalJerk = KickInfo.VerticalJerk - (KickInfo.VerticalJerk * (1 - KickInfo.VerticalJerkRandomness));
        float randomVerticalJerk = Random.Range(KickInfo.VerticalJerk - boundsVerticalJerk, KickInfo.VerticalJerk + boundsVerticalJerk);

        float boundsSideJerk = KickInfo.SideJerk - (KickInfo.SideJerk * (1 - KickInfo.SideJerkRandomness));
        float randomSideJerk = Random.Range(KickInfo.SideJerk - boundsSideJerk, KickInfo.SideJerk + boundsSideJerk);

        int side = Random.Range(0, 2);
        if (side == 0)
            side = -1;

        controller.Rotation((randomSideJerk * side), -randomVerticalJerk);

        while (t < KickInfo.KickLength)
        {
            t += Time.deltaTime;

            float angleY = KickInfo.VerticalMotionCurve.Evaluate((t / KickInfo.KickLength) * KickInfo.VerticalMotionCurve.keys.Last().time) * (KickInfo.VerticalMotion + randomVertical);
            float angleX = KickInfo.SideMotionCurve.Evaluate((t / KickInfo.KickLength) * KickInfo.SideMotionCurve.keys.Last().time) * (KickInfo.SideMotion + randomSide);
            controller.SetMotionY(-angleY);
            controller.SetMotionX(angleX * side);
            yield return new WaitForEndOfFrame();
        }

        controller.SetMotionZero();
    }

    public virtual IEnumerator FlareFade(GameObject flare, float time)
    {
        Color c = flare.GetComponent<MeshRenderer>().material.GetColor("_TintColor");
        float scale = flare.transform.localScale.x;
        float t = 0;
        float delta = 0;
        while (flare != null && c.a > 0.01f)
        {
            delta += Time.deltaTime;
            t = delta / time;
            Color n = Color.Lerp(c, new Color(c.r, c.g, c.b, 0), t);
            float s = Mathf.Lerp(scale, 0, t);
            flare.transform.localScale = new Vector3(s, s, flare.transform.localScale.z);
            flare.GetComponent<MeshRenderer>().material.SetColor("_TintColor", n);
            yield return new WaitForEndOfFrame();
        }
    }
}

[System.Serializable]
public struct FirearmStatsInfo
{
    public FirearmAction Action;
    public float RPM;
    public float BoltWaitTime;
    public float Damage;
    public float BulletVelocity;
}

[System.Serializable]
public struct CameraKickInfo
{
    public float KickLength;
    public float VerticalMotion;
    [Range(0, 1)]
    public float VerticalMotionRandomness;
    public float SideMotion;
    [Range(0, 1)]
    public float SideMotionRandomness;
    public float VerticalJerk;
    [Range(0, 1)]
    public float VerticalJerkRandomness;
    public float SideJerk;
    [Range(0, 1)]
    public float SideJerkRandomness;
    public AnimationCurve VerticalMotionCurve;
    public AnimationCurve SideMotionCurve;
}

public enum FirearmAction
{
    Single,
    Bolt,
    Semi,
    Full,
}
