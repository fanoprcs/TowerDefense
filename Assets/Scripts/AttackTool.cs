using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    arrow, fireball, rock
}

public class AttackTool : MonoBehaviour
{
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private AttackType type;

    public bool destroy = false;
    public int AttackDamage
    {
        get { return attackDamage; }
    }
    public AttackType Type
    {
        get { return type; }
    }
}
