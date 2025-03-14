using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    UIManager uiManager;
    Checkpoints checkPointManager;
    Dictionary<string,int> lapCounts;
    public void OnTriggerEnter2D(Collider2D collision) {
        bool isPlayer = collision.gameObject.CompareTag("Player");
        bool isNPC    = collision.gameObject.CompareTag("NPC");

        if(!isPlayer && !isNPC) {
            return;
        }
        bool gotAllCheckPoints = checkPointManager.GotAllCheckpoints(collision.name);
        if(!gotAllCheckPoints) {
            return;
        }
        lapCounts[collision.name] += 1;
        checkPointManager.ResetCheckpointIndex(collision.name);
        if (isPlayer) {
            uiManager.changeLapRound();
        } else {
            CheckGameOver(collision.name);
        }
    }

    void Start() {
        uiManager = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<UIManager>();
        checkPointManager = GameObject.FindObjectOfType<Checkpoints>();
        lapCounts = new Dictionary<string, int>();
        foreach(string name in checkPointManager.GetAllCarNames()) {
            lapCounts[name] = 0;
        }
    }

    public void CheckGameOver(string name) {
        if(lapCounts[name] == 3) {
            uiManager.loadEndScreen();
        }
    }

    public int GetLapCount(string name) {
        return lapCounts[name];
    }
}
