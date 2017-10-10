using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Head;
    public Transform RecoilMotion;
    public Camera PlayerCamera;
    public Camera FOVCamera;

    private float rotationY = 0;
    private float rotationX = 0;
    private float motionX = 0;
    private float motionY = 0;

    private Vector3 velocity;

    public float Sensitivity = 1;
    public float ActualSensitivity = 1;
    public float MovementSpeed = 1;
    public float MovementAcceleration = 3;
    public float MovementBrake = 7;

    public bool Grounded = false;

    private Rigidbody r;

    public Vector2 GetCurrentRotation()
    {
        return new Vector2(rotationX, rotationY);
    }

	// Use this for initialization
	void Start ()
    {
        r = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Rotation();
        Movement();
    }

    public void ToFOV(float fov, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(IE_fov(fov, speed));
    }

    private IEnumerator IE_fov(float fov, float speed)
    {
        if (PlayerCamera.fieldOfView == fov)
            yield break;

        float t = Time.time;
        float start = PlayerCamera.fieldOfView;
        float length = Mathf.Abs(PlayerCamera.fieldOfView - fov);
        float fracJourney = 0;

        while (fracJourney < 1)
        {
            float distCovered = (Time.time - t) * speed;
            fracJourney = distCovered / length;
            PlayerCamera.fieldOfView = Mathf.Lerp(start, fov, fracJourney);
            FOVCamera.fieldOfView = Mathf.Lerp(start, fov, fracJourney);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Movement()
    {
        velocity = new Vector3(velocity.x, velocity.y + -9.81f * Time.deltaTime, velocity.z);

        RaycastHit h;

        int forward = 0;

        if (Input.GetKey(KeyCode.W)) forward = 1;
        if (Input.GetKey(KeyCode.S)) forward = -1;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) forward = 0;

        if (forward > 0)
        {
            velocity = new Vector3(velocity.x, velocity.y, velocity.z + (MovementAcceleration * Time.deltaTime));
        }
        else if (forward < 0)
        {
            velocity = new Vector3(velocity.x, velocity.y, velocity.z + (-MovementAcceleration * Time.deltaTime));
        }
        else
        {
            if (velocity.x > 0)
            {
                if (velocity.z < MovementBrake * Time.deltaTime)
                    velocity = new Vector3(velocity.x, velocity.y, 0);
                else
                    velocity = new Vector3(velocity.x, velocity.y, velocity.z -= MovementBrake * Time.deltaTime);
            }
            else
            {
                if (velocity.z > -MovementBrake * Time.deltaTime)
                    velocity = new Vector3(velocity.x, velocity.y, 0);
                else
                    velocity = new Vector3(velocity.x, velocity.y, velocity.z += MovementBrake * Time.deltaTime);
            }

            if (Mathf.Abs(velocity.z) < 0.1) velocity = new Vector3(velocity.x, velocity.y, 0);
        }

        int strafe = 0;

        if (Input.GetKey(KeyCode.D)) strafe = 1;
        if (Input.GetKey(KeyCode.A)) strafe = -1;
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) strafe = 0;

        if (strafe > 0)
        {
            velocity = new Vector3(velocity.x + (MovementAcceleration * Time.deltaTime), velocity.y , velocity.z);
        }
        else if (strafe < 0)
        {
            velocity = new Vector3(velocity.x + (-MovementAcceleration * Time.deltaTime), velocity.y, velocity.z);
        }
        else
        {
            if (velocity.x > 0)
            {
                if (velocity.x < MovementBrake * Time.deltaTime)
                    velocity = new Vector3(0, velocity.y, velocity.z);
                else
                    velocity = new Vector3(velocity.x -= MovementBrake * Time.deltaTime, velocity.y, velocity.z);
            }
            else
            {
                if (velocity.x > -MovementBrake * Time.deltaTime)
                    velocity = new Vector3(0, velocity.y, velocity.z);
                else
                    velocity = new Vector3(velocity.x += MovementBrake * Time.deltaTime, velocity.y, velocity.z);
            }

            if (Mathf.Abs(velocity.x) < 0.1) velocity = new Vector3(0, velocity.y, velocity.z);
        }

        Vector3 clamped = Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), MovementSpeed);
        velocity = new Vector3(clamped.x, velocity.y, clamped.z);

        Vector3 localMovement = transform.TransformVector(velocity);

        if (RaycastDown(out h))
        {
            velocity = new Vector3(velocity.x, 0, velocity.z);
            transform.position = new Vector3(transform.position.x + localMovement.x * Time.deltaTime, h.point.y + 1, transform.position.z + localMovement.z * Time.deltaTime);
            Grounded = true;
        }
        else
        {
            Grounded = false;
            transform.position += velocity * Time.deltaTime;
        }
    }

    public void Rotation(float kickX = 0, float kickY = 0)
    {
        rotationY += (-Input.GetAxisRaw("Mouse Y") * ActualSensitivity) + kickY;
        rotationX += (Input.GetAxisRaw("Mouse X") * ActualSensitivity) + kickX;

        rotationY = Mathf.Clamp(rotationY, -90, 80);

        Head.transform.localEulerAngles = new Vector3(rotationY, 0, 0);
        transform.eulerAngles = new Vector3(0, rotationX, 0);
    }

    public void EQCameraYJerk()
    {
        Rotation(motionX, motionY);
        SetMotionZero();
    }

    public void CameraMotion(float kickX, float kickY)
    {
        motionX += kickX;
        motionY += kickY;

        motionY = Mathf.Clamp(motionY, -90, 80);

        RecoilMotion.transform.localEulerAngles = new Vector3(motionY, motionX, 0);
    }

    public void SetMotionX(float angle)
    {
        motionX = angle;
        CameraMotion(motionX, 0);
    }
    public void SetMotionY(float angle)
    {
        motionY = angle;
        CameraMotion(0, motionY);
    }

    public void SetMotionZero()
    {
        RecoilMotion.transform.localEulerAngles = Vector3.zero;
        motionX = 0;
        motionY = 0;
    }

    private bool RaycastDown(out RaycastHit hit)
    {
        RaycastHit h;

        bool b = Physics.Raycast(transform.position, -transform.up, out h, (velocity.magnitude * Time.deltaTime) + 1);

        hit = h;

        return b;
    }
}
