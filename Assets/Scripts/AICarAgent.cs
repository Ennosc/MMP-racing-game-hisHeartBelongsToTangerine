using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AICarAgent : Agent {
    [SerializeField] private Checkpoints checkpoints;
    [SerializeField] private Transform spawnPosition;
    
    private TopDownCarController carDriver;

    private void Awake() {
        carDriver = GetComponent<TopDownCarController>();
    }
    private void Start() {
        checkpoints.onCarCorrectCheckpoint += RewardCorrectCheckpoint;
        checkpoints.onCarWrongCheckpoint += PunishWrongCheckpoint;
    }
    
    private void RewardCorrectCheckpoint(object sender, System.EventArgs e) {
        Checkpoints.CheckpointsEvent checkpointsEvent = (Checkpoints.CheckpointsEvent) e;
        if (checkpointsEvent.car == transform) {
            AddReward(1.0f);
        }
    }

    private void PunishWrongCheckpoint(object sender, System.EventArgs e) {
        Checkpoints.CheckpointsEvent checkpointsEvent = (Checkpoints.CheckpointsEvent) e;
        if (checkpointsEvent.car == transform) {
            AddReward(-1.0f);
        }
    }

    public override void OnEpisodeBegin() {
        transform.position = spawnPosition.position + new Vector3(Random.Range(-5f,+5f), Random.Range(-5f, +5f), 0);
        transform.forward = spawnPosition.forward;
        checkpoints.ResetCheckpointIndex(transform.name);
        carDriver.SetInputVector(new Vector2(0f, 0f));
    }

    public override void CollectObservations(VectorSensor sensor) {
        Vector3 checkpointForward = checkpoints.GetNextCheckpoint(transform.name).transform.forward;
        float directionToCheckpoint = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionToCheckpoint);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float forwardAmount = 0f;
        float turnAmount = 0f;
        switch(actions.DiscreteActions[0]) {
            case 0: forwardAmount = 0; break;
            case 1: forwardAmount = +1f; break;
            case 2: forwardAmount = -1f; break;
        }
        switch(actions.DiscreteActions[1]) {
            case 0: turnAmount = 0; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }
        carDriver.SetInputVector(new Vector2(turnAmount, forwardAmount));
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.W)) {
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.S)){
            forwardAction = 2;
        }
        int turnAction = 0;
        if (Input.GetKey(KeyCode.A)) {
            turnAction = 2;
        }
        if (Input.GetKey(KeyCode.D)) {
            turnAction = 1;
        }
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Wall")) {
            AddReward(-0.5f);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Wall")) {
            AddReward(-0.1f);
        }
    }
}   
