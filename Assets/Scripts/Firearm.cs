using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Firearm : MonoBehaviour
{
    public Camera PlayerCamera;
    public BallisticProjectile BulletPrefab;
    public float BulletVelocityMS;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Fire());
	}

    public IEnumerator Fire()
    {
        yield return new WaitForSeconds(1);
        Transform muzzle = transform.FindChild("Muzzle");

        BallisticProjectile b = Instantiate(BulletPrefab, muzzle.position, muzzle.rotation) as BallisticProjectile;
        b.SetCamera(PlayerCamera);
        b.SetForwardVelocity(BulletVelocityMS);
        StartCoroutine(Fire());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
