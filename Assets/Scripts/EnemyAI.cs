using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * When enabled, the enemy will flee from the player
 */

enum EnemyState { Init, Wandering, Fleeing, Birthing, PrayingToTheElderGhods };

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] float wanderDuration = 0.5f;
    [SerializeField] float fleeDuration = 6f;
    [SerializeField] float wanderSpeed = 3.5f;
    [SerializeField] float fleeSpeed = 5f;
    [SerializeField] Material wanderMaterial;
    [SerializeField] Material fleeMaterial;
    public MeshRenderer leftMeshRenderer, rightMeshRenderer;
    public Material[] wanderMaterials = new Material[1];
    public Material[] fleeMaterials = new Material[1];
    [SerializeField] bool vision360 = false;
    float navigationExpire = float.MinValue;

    Vector3 goal;
    Transform player = default;
    NavMeshAgent navMeshAgent = default;
    EnemyState state = default;
    private NavMeshPath navMeshPath;

    // DEBUG
    Vector3 myPos;
    Vector3 dbgPlayerPos;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        state = EnemyState.Wandering;
        navMeshPath = new NavMeshPath();
        leftMeshRenderer = transform.Find("RepositionBase/LEye").GetComponent<MeshRenderer>();
        rightMeshRenderer = transform.Find("RepositionBase/REye").GetComponent<MeshRenderer>();
        fleeMaterials[0] = fleeMaterial;                            // HACK - these should be static
        wanderMaterials[0] = wanderMaterial;
}



    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Wandering)
        {
            if (CanSeePlayer())
                Flee();
            else if (Time.time >= navigationExpire)
                Wander();
        }
        if (state == EnemyState.Fleeing)
        {
            if (HasReachedDestination())
                Flee(true);
            else if (Time.time >= navigationExpire)
                Wander();
            else
                DebugShowPath();
        }
    }

    bool HasReachedDestination()
    {
        return
            !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);
    }

    void DebugShowPath()
    { 
        for (int i = 0; i<navMeshPath.corners.Length - 1; i++)
            Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.red);
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;
        Vector3 sourcePoint = vision360 ? transform.up : transform.forward;
        Debug.Log("sourcePoint: " + sourcePoint);
        Debug.Log("tPoint: " + transform.position);
        Debug.Log("fPoint: " + transform.forward);
        //Debug.Break();
        //Debug.DrawRay(transform.position + sourcePoint, player.position, Color.green);

        return Physics.Linecast(transform.position + sourcePoint, player.position, out hit) && hit.transform.tag == "Player";
    }

    void Wander()
    {
        state = EnemyState.Wandering;
        SetStateAppearance();
        goal = RandomNavSphere(transform.position, wanderRadius, -1);
        navMeshAgent.SetDestination(goal);
        navMeshAgent.speed = wanderSpeed;
        navigationExpire = Time.time + wanderDuration;
    }


    static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        return NavMesh.SamplePosition(randDirection, out navHit, dist, layermask) ?
            navHit.position :
            origin;
    }




    void Flee(bool isContinuation = false)
    {
        Debug.Log("Flee");

        state = EnemyState.Fleeing;
        SetStateAppearance();

        Vector3 playerPos = player.transform.position;
        Vector3 awayPos = transform.position + (transform.position - playerPos).normalized * 10f;
        int tries = 9;

        do
        {
            goal = RandomNavSphere(awayPos, wanderRadius, -1);
        } while ((!navMeshAgent.CalculatePath(goal, navMeshPath) || !IsPathAcceptable()) && --tries > 0);

        navMeshAgent.SetDestination(goal);
        navMeshAgent.speed = fleeSpeed;

        state = EnemyState.Fleeing;

        if (!isContinuation)
            navigationExpire = Time.time + fleeDuration;
    }


    bool IsPathAcceptable()
    {
        float currentDistanceToPlayer = Vector3.Distance(transform.position, player.position);

        for (int i = 0; i < navMeshPath.corners.Length; i++)
            if (Vector3.Distance(navMeshPath.corners[i], player.position) < currentDistanceToPlayer)
                return false;
        return true;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(goal, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(myPos, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(dbgPlayerPos, 0.5f);
    }


    void SetStateAppearance()
    {
        Material[] mats = state == EnemyState.Wandering ? wanderMaterials : fleeMaterials;
        leftMeshRenderer.materials = mats;
        rightMeshRenderer.materials = mats;
    }
}
