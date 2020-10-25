using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleShader : MonoBehaviour
{

    public bool isEnabled = false;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Toggle();

    }

    public void Toggle()
    {
        isEnabled = !isEnabled;
        //Debug.Log("Polo response: " + (isEnabled ? "ON" : "OFF"));
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
            gameObject.GetComponent<MeshRenderer>().enabled = isEnabled;
    }

}
