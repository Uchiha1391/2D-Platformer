using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    //What to chase
    public Transform target;

    //How many times a second we update the path
    public float updateRate = 2f;

    //The calculated path
    public Path path;

    //The AI's speed per second;
    public float speed = 300f;
    public ForceMode2D forceMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //The max distance from the AI to the waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3f;

    //The waypoint we're currently moving towards
    public int currentWayPoint = 0;




    //Caching
    private Seeker seeker;
    private Rigidbody2D rigidBody;




    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody = GetComponent<Rigidbody2D>();

        if(target==null)
        {
            Debug.Log("Player not found on start");
            return;
        }

        //Start a new path to the target position and return the result to the OnPathComplete function
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        if(target==null)
        {
            //TODO: insert a player search here
            yield return false;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path, did it have an error?: " + p.error);
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(target==null)
        {
            return;
        }

        if(path==null)
        {
            return;
        }

        if(currentWayPoint>=path.vectorPath.Count)
        {
            if(pathIsEnded)
            {
                return;
            }
            Debug.Log("Path has ended");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        direction *= speed * Time.fixedDeltaTime;

        //Move the AI
        rigidBody.AddForce(direction, forceMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);

        if (dist<nextWaypointDistance)
        {
            currentWayPoint++;
            return;
        }
    }



}
