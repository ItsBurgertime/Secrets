using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * When enabled, the enemy will flee from the player
 */

#pragma warning disable 0649,0414

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
    Material[] wanderMaterials = new Material[1];
    Material[] fleeMaterials = new Material[1];

    [Tooltip("View cone in front of enemy. set to 360 to have eyes in the back of his head")]
    [SerializeField] float viewAngle = 360f;

    MeshRenderer leftMeshRenderer, rightMeshRenderer;
    float navigationExpire = float.MinValue;
    Transform visionPoint;

    Vector3 goal;
    Transform player;
    NavMeshAgent navMeshAgent;
    EnemyState state;
    NavMeshPath navMeshPath;
    AudioSource audioSource;
    [SerializeField] AudioClip sfxDeath;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        state = EnemyState.Wandering;
        navMeshPath = new NavMeshPath();
        leftMeshRenderer   = transform.Find("RepositionBase/LEye").GetComponent<MeshRenderer>();
        rightMeshRenderer  = transform.Find("RepositionBase/REye").GetComponent<MeshRenderer>();
        visionPoint        = transform.Find("RepositionBase/VisionPoint").transform;
        fleeMaterials[0]   = fleeMaterial;                            // HACK - these should be static
        wanderMaterials[0] = wanderMaterial;
        audioSource = player.gameObject.GetComponent<AudioSource>();  // HACKY
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
        Vector3 playerDir = player.position - transform.position;
        
        // The angle returned is the unsigned angle between the two vectors.
        // This means the smaller of the two possible angles between the two vectors is used.
        // The result is never greater than 180 degrees.

        float angle = Vector3.Angle(transform.forward, playerDir) * 2;

        Debug.DrawLine(visionPoint.position, player.position, Color.green);
        return angle < viewAngle && Physics.Linecast(visionPoint.position, player.position, out hit) && hit.transform.tag == "Player";
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

        if (Vector3.Distance(goal, player.position) < currentDistanceToPlayer)
            return false;

        for (int i = 0; i < navMeshPath.corners.Length; i++)
            if (Vector3.Distance(navMeshPath.corners[i], player.position) < currentDistanceToPlayer)
                return false;
        return true;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(goal, 0.25f);

    }


    void SetStateAppearance()
    {
        Material[] mats = state == EnemyState.Wandering ? wanderMaterials : fleeMaterials;
        leftMeshRenderer.materials = mats;
        rightMeshRenderer.materials = mats;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sfxDeath);
            Destroy(gameObject);
        }
    }
}
