using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public List<CheckpointController> checkpointList;
    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<CheckpointController>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointController checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointController>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointList.Add(checkpointSingle);

        }
    }

    public void ResetColor() {
        Color defaultColor = Color.green;
        defaultColor.a = 0.5f;
        foreach (CheckpointController checkpointSingle in checkpointList) {
            Renderer checkpointRen = checkpointSingle.GetComponent<Renderer>();
            checkpointRen.material.color = defaultColor;
        }
    }
    public void CheckpointTriggered(CheckpointController checkpointController, bikeController bike) {
        Color correctColor = Color.yellow;
        correctColor.a = 0.5f;
        Color wrongColor = Color.red;
        wrongColor.a = 0.5f;

        if (checkpointList.IndexOf(checkpointController) == bike.checkpointProgress) {
            Renderer checkpointRen = checkpointController.GetComponent<Renderer>();
            checkpointRen.material.color = correctColor;

            bike.EnteredCheckpointCorrectly(false);
            bike.checkpointProgress++;
        } else if (checkpointList.IndexOf(checkpointController) == checkpointList.Count) {
            bike.EnteredCheckpointCorrectly(true);
        } else {
            bike.EnteredWrongCheckpoint();
            Renderer checkpointRen = checkpointController.GetComponent<Renderer>();
            checkpointRen.material.color = wrongColor;
            // Renderer checkpointRen = checkpointController.GetComponent<Renderer>();
            // checkpointRen.material.color = correctColor;

            // bike.EnteredCheckpointCorrectly(false);
            // bike.checkpointProgress++;
        }
    }
}
