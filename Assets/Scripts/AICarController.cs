using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AICarController : MonoBehaviour {


    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 1.5f;
    public float maxSpeed = 20;

    [Header("Booster settings")]
    public float boostamountmaxspeed = 10;
    public float boostamountacceleration = 5;
    public float boosterTime = 2;

    float velocityVsUp = 0;

    //map boundary
    [Header("Map boundries")]
    public float minX = -125f;
    public float maxX = 50f;
    public float minY = -50f;
    public float maxY = 125f;

    Rigidbody2D carRigidbody2D;
    CarSurfaceHandler carSurfaceHandler;
    [SerializeField] private Checkpoints checkpoints;
    private Transform target;



     void Awake() {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carSurfaceHandler = GetComponent<CarSurfaceHandler>();
     }

    private void Start() {
        checkpoints.onCarCorrectCheckpoint += TargetNextCheckpoint;
        target = checkpoints.GetNextCheckpoint(transform.name).transform;
    }

    private void TargetNextCheckpoint(object sender, EventArgs e) {
        Checkpoints.CheckpointsEvent checkpointsEvent = (Checkpoints.CheckpointsEvent) e;
        if(checkpointsEvent.car == transform) {
            target = checkpoints.GetNextCheckpoint(transform.name).transform;
        }
    }

    void FixedUpdate() {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();

        ClampPosition();
    }

    void ApplyEngineForce()
    {
        //Calculate how much "forward" we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        //Limit so we cannot go faster than the max speed in the "forward" direction
        if (velocityVsUp > maxSpeed) {
            return;
        }

        //Limit so we cannot go faster in any direction while accelerating
        if (carRigidbody2D.velocity.sqrMagnitude > (maxSpeed * maxSpeed)) {
            return;
        }
        /*

        switch (GetSurface()){
            case Surface.SurfaceTypes.Grass:
                carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 4.0f, Time.fixedDeltaTime * 3);
                break;
            case Surface.SurfaceTypes.Oil:
                carRigidbody2D.drag = 0;
                break;
        }
        */

        Vector2 engineForceVector = transform.up * accelerationFactor;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering() {
        //Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = carRigidbody2D.velocity.magnitude / 8;
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Fetch the first GameObject's position
        Vector2 currentPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Vector2 targetPosition = new Vector2(target.localPosition.x, target.localPosition.y);
        Vector2 deltaPositions = targetPosition - currentPosition;
        float angle = Vector2.Angle(deltaPositions.normalized, transform.up);
        float angleDir = -Math.Sign(Vector2.SignedAngle(deltaPositions, transform.up));


        //Log values of Vectors and angle in Console
        print("MyFirstVector: " + deltaPositions);
        print("MySecondVector: "  + transform.up);
        print("Angle Between Objects: " + angle);
        print("SIgned Angle Between Objects: " + angleDir);
        print("Currentcheckpoint: " + checkpoints.GetNextCheckpoint(transform.name).name);

        if (angle < 5.0f) {
            print("Close");
        } else {
            carRigidbody2D.MoveRotation(angleDir* angle);
        }
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        float currentDriftFactor = driftFactor;

        //Apply more drag depending on surface
        switch (GetSurface())
        {
            case Surface.SurfaceTypes.Grass:
                currentDriftFactor *= 1.05f;
                break;
            case Surface.SurfaceTypes.Oil:
                currentDriftFactor = 1.00f;
                break;
        }

        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void useRocketBooster()
    {
        maxSpeed = maxSpeed + boostamountmaxspeed;
        accelerationFactor = accelerationFactor + boostamountacceleration;
        Invoke("resetSpeed", boosterTime);
    }

    private void resetSpeed()
    {
        accelerationFactor = 30.0f;
        maxSpeed = 20;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }

    public Surface.SurfaceTypes GetSurface()
    {
        return carSurfaceHandler.GetCurrentSurface();
    }

    void ClampPosition()
    {
        
        //Clamp the car's position within the specified boundaries
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY)
        );
        //Debug.Log($"Original position: {transform.position}");
        //Debug.Log($"Clamping to: {clampedPosition}");

        transform.position = clampedPosition;
    }

}
