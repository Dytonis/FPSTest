using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FPS.Physics
{
    public class Bullet : BallisticProjectile
    {
        public FlareInfo Flare;
        public DebrisInfo Debris;

        public override void OnHit(RaycastHit hit)
        {
            if (Debris.Object)
            {
                int amt = Random.Range(Debris.Count - Debris.CountRandomness, Debris.Count + Debris.CountRandomness);
                for (int i = 0; i < amt; i++)
                {
                    GameObject d = Instantiate(Debris.Object, hit.point, Debris.Object.transform.rotation) as GameObject;
                    d.transform.localScale = Random.Range(Debris.Size - Debris.SizeRandomness, Debris.Size + Debris.SizeRandomness).ToVector();
                    d.transform.rotation = Quaternion.FromToRotation(d.transform.forward, hit.normal);
                    d.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(
                        Random.Range(-Debris.Velocity - Debris.VelocityRandomness, Debris.Velocity + Debris.VelocityRandomness), 
                        Random.Range(-Debris.Velocity - Debris.VelocityRandomness, Debris.Velocity + Debris.VelocityRandomness), 
                        Random.Range(70, Debris.Velocity + Debris.VelocityRandomness)));
                    d.GetComponent<MeshRenderer>().material.color = hit.transform.GetComponent<MeshRenderer>().material.color;
                    Destroy(d.gameObject, Random.Range(Debris.Life - Debris.LifeRandomness, Debris.Life + Debris.LifeRandomness));
                }
            }
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public struct FlareInfo
    {
        public float Size;
        public GameObject Object;
        public int Count;
        public float Life;
    }

    [System.Serializable]
    public struct DebrisInfo
    {
        public float Size;
        public float SizeRandomness;
        public GameObject Object;
        public int Count;
        public int CountRandomness;
        public float Life;
        public float LifeRandomness;
        public float Velocity;
        public float VelocityRandomness;
    }
}
