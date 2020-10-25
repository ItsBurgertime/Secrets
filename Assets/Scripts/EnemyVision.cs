using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;

/** Detects player */
public class EnemyVision : MonoBehaviour
{

    [SerializeField] float radius = 2f;
    [SerializeField] float range = 10f;
    [SerializeField] float eyeHeight = 1.5f;
    EnemyAI enemyAI = default;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * range;
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(pos, forward, Color.green);
    }

}
