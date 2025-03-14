using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour {
    private Checkpoints checkpoints;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<CarInputHandler>(out CarInputHandler car)) {
            print($"Checkpoint hit: {car.name}!");
            checkpoints.CarThroughCheckpoint(this, car.transform);
        }
    }

    public void SetTrackCheckpoint(Checkpoints checkpoints) {
        this.checkpoints = checkpoints;
    }
}
