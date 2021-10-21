using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : ScriptableObject
{
    public GameObject prefab;
    public float maxHealth;
    public bool canWalk;
    public int walkSpeed;
    public bool canRun;
    public int runSpeed;
    public bool canJump;
    public int jumpHeight;
    public string charName;
}
