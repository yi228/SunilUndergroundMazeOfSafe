using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AttackEvent : MonoBehaviour
{
    MonsterController Owner;
    private void Start()
    {
        Owner = transform.parent.GetComponent<MonsterController>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Owner._monsterType == Define.MonsterType.Rush && collision.gameObject.tag == "Player" && Owner._rushAttackTimer >= Owner.Stat.AttackSpeed)
        {
            Owner._rushAttackTimer = 0f;
            StartCoroutine(Owner.CoRushAttack());
        }
    }
}