using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;
    public bool turnTaken;
    public int health = 100;
    public int AttackDmg;
    public int MoveSpeed;

}