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
	public Rigidbody m_targetRb;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        m_AgentRb = GetComponent<Rigidbody>();
        m_RayPer = GetComponent<RayPerception>();
    }

    void start()
    {
    	SetReward(1.0f);
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
            var P = m_RayPer.Perceive(rayDistance, rayAngles, detectableObjects);
            var P1 = m_RayPer.Perceive(rayDistance, rayAngles1, detectableObjects);
            var P2 = m_RayPer.Perceive(rayDistance, rayAngles2, detectableObjects);
            AddVectorObs(m_targetRb.position);
            AddVectorObs(P);
            AddVectorObs(P1);
            AddVectorObs(P2);
            // AddVectorObs(transform.InverseTransformDirection(m_AgentRb.velocity));
            AddVectorObs(this.transform.position);
            // foreach (float per in P)
	           //  Debug.Log(per.ToString());
	        // Debug.Log(P.Count * 3);//
	        // Debug.Log(transform.InverseTransformDirection(m_AgentRb.velocity));
	        // Debug.Log(transform.InverseTransformDirection(transform.position));
				//String.Join("", new List<int>(P).ConvertAll(i => i.ToString()).ToArray()));
        }
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        // var rotateDir = Vector3.zero;
        dirToGo.x = act[0];
        dirToGo.z = act[1];


        // Debug.Log("" + act[0] + "," + act[1]);
        // var action = Mathf.FloorToInt(act[0]);
        // switch (action)
        // {
        //     case 0:
        //         dirToGo = transform.forward * 1f;
        //         break;
        //     case 1:
        //         dirToGo = transform.forward * -1f;
        //         break;
        // }
        // m_AgentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
        // // if(dirToGo != transform.forward && dirToGo != Vector3.zero)
	       //  // Debug.Log(dirToGo);

        // action = Mathf.FloorToInt(act[1]);
        // switch (action)
        // {
        //     case 0:
        //         dirToGo = transform.right * 1f;
        //         break;
        //     case 1:
        //         dirToGo = transform.right * -1f;
        //         break;
        // }
        // transform.Rotate(rotateDir, Time.deltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 5f, ForceMode.VelocityChange);
        // if(dirToGo != transform.forward && dirToGo != Vector3.zero)
        Debug.Log(dirToGo);
    }

    public override void AgentAction(float[] vectorAction)
    {
        AddReward(-1f / agentParameters.maxStep - Math.Abs(Vector3.Distance(m_AgentRb.position, m_targetRb.position))/50000);
        // Debug.Log(Math.Abs(Vector3.Distance(m_AgentRb.position, m_targetRb.position))/100);
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
        // var enumerable = Enumerable.Range(0, 9).OrderBy(x => Guid.NewGuid()).Take(9);
        // var items = enumerable.ToArray();

        m_AgentRb.velocity = Vector3.zero;
        transform.position = (new Vector3(Random.Range(3, 47), transform.position.y, Random.Range(-47, -3)));
        // transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("target"))
        {
            Done();
            SetReward(2f);
            Debug.Log("Done");
            m_targetRb.velocity = Vector3.zero;
            m_targetRb.transform.position = (new Vector3(Random.Range(-3, -47), transform.position.y, Random.Range(3, 47)));
        }
        if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(-0.2f);
        }
    }

    public override void AgentOnDone()
    {
    }
}
