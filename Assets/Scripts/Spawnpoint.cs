using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    Player,
    Enemy
}

//basically just data and transform
public class Spawnpoint : MonoBehaviour
{
    public SpawnType spawnType;
}
