using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharModel", menuName = "Char")]
public class CharModel : ScriptableObject
{

    public string name;
    public float speed;
    public GameObject character;

}
