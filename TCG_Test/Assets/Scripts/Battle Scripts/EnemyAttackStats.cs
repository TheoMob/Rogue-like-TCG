using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMove", menuName = "Moves")]
public class EnemyAttackStats : ScriptableObject
{

    public float Damage;
    public float Heal;
    public MyEnum cardEffects = new MyEnum();
    public AnimEnum AttackAnimation = new AnimEnum();

    public enum MyEnum
    {
        none,
        Confusion,
        batatinha
    };

    public enum AnimEnum
    {
         Attack,
         Attack2
    };


}
