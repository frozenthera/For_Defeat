using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrap : PlayerSkill
{
    public GameObject TrapObjectPrefab;
    public override IEnumerator _OnSkillActive()
    {
        GameObject go = Instantiate(TrapObjectPrefab, origin.transform.position, Quaternion.identity);
        go.GetComponent<TrapObject>().trapHP = (int)(origin.GetComponent<PlayerController>().CurAngerGauge / 333) + 2;
        yield return null;
    }
}