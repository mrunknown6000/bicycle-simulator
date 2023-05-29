using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class bikeController : Agent
{
    #region Variable
    // Acceleration/Turn/Brake Rate
    public float accelerationRate = 40f;
    public float brakeRate = 80f;
    public float turnAuthority = 60f;
    public int checkpointProgress;
    // Component Data
    private float timer;
    [SerializeField] private Vector3 ogPos;
    [SerializeField] private Quaternion ogRot;
    [SerializeField] private WheelCollider backWheel;
    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private Transform wheelMesh;
    [SerializeField] private Transform handleMesh;
    [SerializeField] private GameControl gameControl;
    #endregion

    private void Start()
    {
        ogPos = transform.position;
        ogRot = transform.rotation;
    }
    public override void OnEpisodeBegin()
    {
        timer = Time.time;
        transform.localPosition = ogPos;
        transform.localRotation = ogRot;

        gameControl.ResetColor();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation.z);
        Vector3 nextCheckpoint = gameControl.checkpointList[checkpointProgress].transform.forward;
        float directionDot = Vector3.Dot(transform.forward, nextCheckpoint);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float accelerate = actions.ContinuousActions[0];
        float braking = actions.ContinuousActions[1];
        float turnAngle = actions.ContinuousActions[2];

        acceleration(accelerate);
        brake(braking);
        turn(turnAngle);

        // Time Limit:)
        // if (Time.time - timer >= 5)
        // {
        //     timer = Time.time;
        //     AddReward(10 - (Math.Abs(this.transform.rotation.z) / 9));
        // }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Convert.ToInt16(Input.GetKey(KeyCode.W));
        continuousActions[1] = Convert.ToInt16(Input.GetKey(KeyCode.S));
        continuousActions[2] = Input.GetAxis("Horizontal");

    }

    public void EnteredCheckpointCorrectly(bool isEnd)
    {
        AddReward(10f);
        if (isEnd)
        {
            EndEpisode();
        }
    }

    public void EnteredWrongCheckpoint() {
        AddReward(-2f);
    }

    #region Actions
    void acceleration(float amount)
    {
        backWheel.motorTorque += amount * accelerationRate * Time.deltaTime;
    }
    void brake(float amount)
    {
        if (backWheel.motorTorque > Math.Sqrt(brakeRate))
        {
            backWheel.motorTorque -= amount * brakeRate * Time.deltaTime;
        }
    }
    void turn(float amount)
    {
        if (-90f <= frontWheel.steerAngle && frontWheel.steerAngle <= 90f) {
            frontWheel.steerAngle += amount * turnAuthority * Time.deltaTime;
            wheelMesh.Rotate(wheelMesh.position, amount * turnAuthority * -1 * Time.deltaTime);
            handleMesh.Rotate(Vector3.forward, amount * turnAuthority * Time.deltaTime);
        }
    }
    #endregion
}
