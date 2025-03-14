using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoints : MonoBehaviour {
    private string PLAYER_NAME = "Car";

    private UIManager uiManager;
    private Dictionary<string,bool> gotAllCheckpoints; 
    private List<CheckpointSingle> checkpoints;
    private Dictionary<String,Transform> cars;
    private Dictionary<String,int> nextCheckpointIndecies;
    private Dictionary<string,int> lapCounts;
    public event EventHandler onCarCorrectCheckpoint;

    public event EventHandler onCarWrongCheckpoint;
    public class CheckpointsEvent: EventArgs {
        public Transform car;
        public CheckpointsEvent(Transform car) {
            this.car = car;
        }
    }
    private void Awake() {
        checkpoints = new List<CheckpointSingle>();
        nextCheckpointIndecies = new Dictionary<string, int>();
        cars = new Dictionary<string,Transform>();
        gotAllCheckpoints = new Dictionary<string,bool>();
        uiManager = GameObject.FindObjectOfType<UIManager>();
        lapCounts = new Dictionary<string, int>();
        InitCarDictionary();
        InitCheckpointList();
        InitNextChekpointIndexList();
        InitGotAllCheckpoints();
        InitLapCounts();
    }
    public CheckpointSingle GetNextCheckpoint(String name) {
        return checkpoints[nextCheckpointIndecies[name]];
    }

    public int GetLapCount(String name) {
        return lapCounts[name];
    }

    public Transform GetCarTransfom(String name) {
        return cars[name];
    }

    public CheckpointSingle GetFirstCheckpoint() {
        return checkpoints.First();
    }

    public int GetIndexOfCar(String nameOfCar){
        return nextCheckpointIndecies[nameOfCar];
    }

    public int MaxCheckpoints() {
        return checkpoints.Count;
    }

    public void ResetCheckpointIndex(String carname) {
        nextCheckpointIndecies[carname] = 0;
        gotAllCheckpoints[carname] = false;
    }

    public String[] GetAllCarNames() {
        return cars.Keys.ToArray();
    }

    private void InitCarDictionary() {
        TopDownCarController[] carGameObjects = (TopDownCarController[]) GameObject.FindObjectsOfType(typeof(TopDownCarController));
        foreach(TopDownCarController car in carGameObjects) {
            cars.Add(car.name, car.transform);
        }
    }

    private void InitCheckpointList() {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        foreach(Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpoint = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpoint.SetTrackCheckpoint(this);
            checkpoints.Add(checkpoint);
        }
    }

    private void InitNextChekpointIndexList() {
        foreach(KeyValuePair<String,Transform> car in cars) {
            nextCheckpointIndecies[car.Key] = 0;
        }
    }

    private void InitGotAllCheckpoints() {
        foreach(KeyValuePair<String,Transform> car in cars) {
            gotAllCheckpoints[car.Key] = false;
        }
    }

    private void InitLapCounts() {
        foreach(KeyValuePair<String,Transform> car in cars) {
            lapCounts[car.Key] = 0;
        }
    }

    public bool GotAllCheckpoints(string name) {
        return gotAllCheckpoints[name];
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpoint, Transform car) {
        int checkpointIndex = checkpoints.IndexOf(checkpoint);
        int nextCheckpointIndex = nextCheckpointIndecies[car.name];
        CheckpointsEvent checkpointsEvent = new CheckpointsEvent(car);

        if (checkpointIndex == nextCheckpointIndex) {
            nextCheckpointIndecies[car.name] = nextCheckpointIndex + 1;
            if(IsLapDone(car.name)) {
                FinishLap(car.name);
            }
            print("newCheckpoint index: " + checkpointIndex);
            onCarCorrectCheckpoint?.Invoke(this, checkpointsEvent);
        } else {
            onCarWrongCheckpoint?.Invoke(this, checkpointsEvent);
        }
    }

    private void FinishLap(string name) {
        nextCheckpointIndecies[name] = 0;
        lapCounts[name] += 1;
        if (name == PLAYER_NAME) {
            uiManager.changeLapRound();
        } else {
            CheckGameOverNPC(name);
        }
    }

    public void CheckGameOverNPC(string name) {
        if(lapCounts[name] == 3) {
            uiManager.loadEndScreenNPC();
        }
    }

    private bool IsLapDone(string name) {
        return nextCheckpointIndecies[name] == MaxCheckpoints();
    }

    private void OnDrawGizmos() {
        if (this.transform.childCount < 2)
            return;

        for (int i = 0; i < this.transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild(i + 1).position);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.GetChild(this.transform.childCount - 1).position, this.transform.GetChild(0).position);
    }
    public string Description() {
        return string.Format("There are {0} checkpoints.", this.transform.childCount);
    }
}