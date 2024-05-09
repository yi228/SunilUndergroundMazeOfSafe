using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public int MaxAttack;
    public int MaxHp;
    public int hp;
    public int Hp { get { return hp; } set { hp = Mathf.Clamp(value, 0, MaxHp); } }
    public int attack;
    public int Attack { get { return attack; } set { attack = Mathf.Clamp(value, 0, MaxAttack); } }
    public int Defense;
    public float MoveSpeed;
    // 일반 공격 딜레이
    public float AttackSpeed;
    // 스킬 쿨타임
    public float SkillSpeed;
}
