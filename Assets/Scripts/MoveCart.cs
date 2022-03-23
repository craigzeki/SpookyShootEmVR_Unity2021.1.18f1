
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCart : MonoBehaviour
{

    [SerializeField] private GameObject waypoints;
    [SerializeField] private float maxSpeed = 1.0f;
    [SerializeField] private float waypointProximity = 0.05f;

    private UnityEngine.AI.NavMeshAgent agent;
    private List<Transform> points = new List<Transform>();
    public int nextPoint = 0;

    void Start()
    {

        //add all the objects to the list
        points.AddRange(waypoints.GetComponentsInChildren<Transform>());
        if (points.Count > 0)
        {
            points.RemoveAt(0); //remove the parent transform as this is not used as a waypoint
        }
        //fetch the navmesh agent component
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //Can turn off autobraking to not stop between waypoints
        agent.autoBraking = false;
        agent.enabled = true;

        StopMoving();
        SetNextPoint();
    }

    void SetNextPoint()
    {
        //guard conditions
        if (points.Count == 0) return;
        if (!agent.enabled) return;
        if (!agent.isOnNavMesh) return;

        //stop moving once made it around all points
        if (nextPoint >= points.Count)
        {
            StopMoving();
            nextPoint = 0;
        }

        //set the next point to move to
        agent.SetDestination(points[nextPoint].position);

        nextPoint++;
    }

    public void StartMoving()
    {
        agent.speed = maxSpeed;
    }

    public void StopMoving()
    {
        agent.speed = 0;
    }

    void Update()
    {
        //guard conditions
        if (points.Count == 0) return;
        if (!agent.enabled) return;
        if (!agent.isOnNavMesh) return;

        //check for proximity to waypoint
        //added this as the center point of the navmesh agent has to get to the waypoint
        //it is sometimes deirable to star turning and moving toward the next point before
        if (agent.remainingDistance < waypointProximity)
        {
            SetNextPoint();
        }

    }

}
