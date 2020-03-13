using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using MLAgents;

public class NavAgent : Agent
{
    Rigidbody m_AgentRb;
    RayPerception m_RayPer;
    public bool useVectorObs;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        m_AgentRb = GetComponent<Rigidbody>();
        m_RayPer = GetComponent<RayPerception>();
    }

    public override void CollectObservations()
    {
        if (useVectorObs)
        {
            const float rayDistance = 35f;
            float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
            float[] rayAngles1 = { 25f, 95f, 165f, 50f, 140f, 75f, 115f };
            float[] rayAngles2 = { 15f, 85f, 155f, 40f, 130f, 65f, 105f };

            string[] detectableObjects = { "target", "wall" };
            // AddVectorObs();
            // AddVectorObs(m_RayPer.Perceive(rayDistance, rayAngles, detectableObjects));
            // AddVectorObs(m_RayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, 5f));
            // AddVectorObs(m_RayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 10f));
            // AddVectorObs(transform.InverseTransformDirection(m_AgentRb.velocity));
            // AddVectorObs(transform.InverseTransformDirection(transform.position));
            string tmp = string.Join(" ", m_RayPer.Perceive(rayDistance, rayAngles, detectableObjects));
            Debug.Log(tmp);
        }
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        // var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                dirToGo = transform.right * 1f;
                break;
            case 4:
                dirToGo = transform.right * -1f;
                break;
        }
        // transform.Rotate(rotateDir, Time.deltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
    }

    public override void AgentAction(float[] vectorAction)
    {
        AddReward(-1f / agentParameters.maxStep);
        MoveAgent(vectorAction);
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { 3 };
        }
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { 1 };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { 4 };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { 2 };
        }
        return new float[] { 0 };
    }

    public override void AgentReset()
    {
        var enumerable = Enumerable.Range(0, 9).OrderBy(x => Guid.NewGuid()).Take(9);
        var items = enumerable.ToArray();

        m_AgentRb.velocity = Vector3.zero;
        transform.position = (new Vector3(Random.Range(3, 47), transform.position.y, Random.Range(-47, -3)));
        // transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("target"))
        {
            SetReward(2f);
            Done();
            Debug.Log("Done");
        }
    }

    public override void AgentOnDone()
    {
    }
}
