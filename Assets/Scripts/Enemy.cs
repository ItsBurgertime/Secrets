using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]MeshRenderer realMeshRenderer;
    public bool poloOnCooldown = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && realMeshRenderer.enabled)
        {
            realMeshRenderer.enabled = false;
        }
        {

            realMeshRenderer.enabled = true;
        }
    }


}
