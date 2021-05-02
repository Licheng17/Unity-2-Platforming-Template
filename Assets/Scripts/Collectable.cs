using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Collectable : MonoBehaviour
{
    public string collectableName; // name of collectable
    public string description; // description of the collection
    public GameObject player;
    public abstract void Use(); // will have different functions for the different subclasses
}
