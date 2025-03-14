using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 1.5f;
    public float maxSpeed = 20;

    [Header("Booster settings")]
    public float boostamountmaxspeed = 10;
    public float boostamountacceleration = 5;
    public float boosterTime = 2;

    //Local variables
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    //map boundary
    [Header("Map boundries")]
    public float minX = -125f;
    public float maxX = 50f;
    public float minY = -50f;
    public float maxY = 125f;

    [Header("Slime settings")]
    public float slimeSlowDuration = 3.0f; // Dauer der Verlangsamung
    public float slimeMaxSpeed = 5; // Max Geschwindigkeit auf Schleim
    public float slimeAccelerationFactor = 10; // Beschleunigungsfaktor auf Schleim



    //Components
    Rigidbody2D carRigidbody2D;
    CarSurfaceHandler carSurfaceHandler;



    //Awake is called when the script instance is being loaded
     void Awake()
     {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carSurfaceHandler = GetComponent<CarSurfaceHandler>();
     }

    //Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //Frame-rate independent for physics calculations
    void FixedUpdate()
    {
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
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        //Limit so we cannot go faster than the 50% speed in the "reverse" direction
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;            
        
        //Limit so we cannot go faster in any direction while accelerating
        if (carRigidbody2D.velocity.sqrMagnitude > (maxSpeed * maxSpeed) && accelerationInput > 0)
            return;

        //Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 0, Time.fixedDeltaTime * 10);

        //Apply more drag depending on surface
        switch (GetSurface())
        {
            case Surface.SurfaceTypes.Grass:
                carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 4.0f, Time.fixedDeltaTime * 3);
                break;
            case Surface.SurfaceTypes.Oil:
                carRigidbody2D.drag = 0;
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
        }

        //Create a force for Engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //Apply force and push forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        //Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);

        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        //Apply steering by rotating the car object
        carRigidbody2D.MoveRotation(rotationAngle);
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

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
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

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();

        isBraking = false;

        //check if we move forward and brake
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
        {
            return true;
        }
        
        return false;

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
