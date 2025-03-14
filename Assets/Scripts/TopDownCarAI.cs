using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCarAI : MonoBehaviour {
    TopDownCarController topDownCarController;
    Transform target;
    [SerializeField] private Checkpoints checkpoints;
    [SerializeField] private float maxSpeed;


    void Awake() {
        topDownCarController = GetComponent<TopDownCarController>();
        topDownCarController.maxSpeed = maxSpeed;
    }
    private void Start() {
        checkpoints.onCarCorrectCheckpoint += TargetNextCheckpoint;
        target = checkpoints.GetNextCheckpoint(transform.name).transform;
    }

    private void TargetNextCheckpoint(object sender, EventArgs e) {
        Checkpoints.CheckpointsEvent checkpointsEvent = (Checkpoints.CheckpointsEvent) e;
        int checkpointIndex = checkpoints.GetIndexOfCar(transform.name);
        if(checkpointsEvent.car != transform) {
            return;
        }
        target = checkpoints.GetNextCheckpoint(transform.name).transform;
        print("new checkpoint " + target.name);
    }

    void FixedUpdate() {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = TurnTorwardsTarget();
        inputVector.y = 1.0f;
        topDownCarController.SetInputVector(inputVector);
    }

    float TurnTorwardsTarget() {
        Vector2 currentToTarget = target.position - transform.position;
        currentToTarget.Normalize();

        float angleToTarget = Vector2.SignedAngle(transform.up, currentToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);
        return steerAmount;
    }

    float ApplyThrottleBrake(float steeringAmount) {
        return 1.0f - Mathf.Abs(steeringAmount) / 1.0f;
    }
}
