using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ScoreboardSystem : MonoBehaviour {
    [SerializeField] private Checkpoints checkpoints;
    private UIManager uiManager;

    private const String PLAYER_NAME = "Car";
    
    void Awake() {
        uiManager  = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<UIManager>();
    }


    void Update() {
        int placement = GetCurrentPlacementOfPlayer();
        uiManager.ChangePlacement(placement);
        print("placement: " + placement);
    }

    private int GetCheckpointIndex(string name) {
        return checkpoints.GetIndexOfCar(name) 
            + checkpoints.MaxCheckpoints() * checkpoints.GetLapCount(name);
    }

    private int GetCurrentPlacementOfPlayer () {
        string[] carNames = checkpoints.GetAllCarNames();
        int playerChekpointIndex = GetCheckpointIndex(PLAYER_NAME);
        float playerDistanceToCheckpoint = GetDistanceToNextCheckpoint(PLAYER_NAME);
        int placement = 1;
        Dictionary<string,int> namedCarCheckpointIndecies = new Dictionary<string, int>();

        foreach(string name in carNames) {
            if(name == PLAYER_NAME) {
                continue;
            }
            namedCarCheckpointIndecies.Add(
                name,
                GetCheckpointIndex(name)
            );
        }

        List<int> otherCarCheckpointIndecies = namedCarCheckpointIndecies.Values.ToList();
        otherCarCheckpointIndecies.Sort();
        otherCarCheckpointIndecies.Reverse();

        otherCarCheckpointIndecies.ForEach((x) => print($"ohter: {x}"));
        print("player : " + playerChekpointIndex);

        foreach (int checkpointIndex in otherCarCheckpointIndecies) {
            if(checkpointIndex > playerChekpointIndex) {
                placement++;
                continue;
            }
        }

        Dictionary<string,int> namedCarSameCheckpoint = namedCarCheckpointIndecies.Where(
            (x) => x.Value == playerChekpointIndex
        ).ToDictionary(x => x.Key, x => x.Value);


        foreach (KeyValuePair<string, int> otherCar in namedCarSameCheckpoint) {
            float otherCarDistanceToCheckpoint = GetDistanceToNextCheckpoint(
                otherCar.Key
            );
            if(otherCarDistanceToCheckpoint < playerDistanceToCheckpoint) {
                placement++;
            }
        }
        return placement;
    }

    private float GetDistanceToNextCheckpoint(String name) {
        Vector2 carPostition = checkpoints.GetCarTransfom(name).localPosition;
        Vector2 checkpointPosition = checkpoints.GetNextCheckpoint(name).transform.localPosition;
        return Vector2.Distance(carPostition, checkpointPosition);
    }

}
