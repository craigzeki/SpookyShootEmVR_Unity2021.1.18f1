
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

	UnityEngine.AI.NavMeshAgent agent;
	public GameObject waypoints;
	// List or Array of points- starts at number 0

	private List<Transform> points = new List<Transform>();
	public int destPoint = 0;
	public bool random;
	[SerializeField] Vector3 hitLocation = Vector3.zero;
	//private int debounce = 200;

	// Use this for initialization
	void Start()
	{
		
		points.AddRange(waypoints.GetComponentsInChildren<Transform>());
		if(points.Count > 0)
        {
			points.RemoveAt(0); //remove the parent transform
        }
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.enabled = false;
		//Can turn off autobraking to not stop between waypoints
		agent.autoBraking = false;

		UnityEngine.AI.NavMeshHit closestHit;
		if ( UnityEngine.AI.NavMesh.SamplePosition(transform.position, out closestHit, 500, 1))
        {
			hitLocation = closestHit.position;
			//transform.position = closestHit.position;
			agent.enabled = true;
        }

		StopMoving();
		GotoNextPoint();
	}

	void GotoNextPoint()
	{
		
		if (points.Count == 0)
		{
			return;
		}
		if(agent.enabled)
		{
			if (agent.isOnNavMesh)
			{

				if (destPoint >= points.Count)
				{
					StopMoving();
					destPoint = 0;
				}
				agent.SetDestination(points[destPoint].position);
                //if goes higher than the total number of waypoints -> go back to start of array
                //destPoint = (destPoint + 1) % points.Count;
                destPoint++;
                
			}
		}
		
		
	}

	public void StartMoving()
    {
		agent.speed = 1;
    }

	public void StopMoving()
    {
		agent.speed = 0;
    }

	// Update is called once per frame
	void Update()
	{
		if(agent.enabled)
        {
			if (agent.isOnNavMesh)
			{
				//if (agent.remainingDistance < 0.01f && debounce-- <= 0)

                    if (agent.remainingDistance < 0.05f)
                    {

					//debounce = 200;
					GotoNextPoint();
					

				}
                
			}
		}
		
		
	}

	IEnumerator WaitAndMove()
    {
		yield return new WaitForSeconds(5);
		GotoNextPoint();
    }


}
