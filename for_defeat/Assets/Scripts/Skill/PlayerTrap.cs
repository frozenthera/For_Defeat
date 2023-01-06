using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrap : Skill
{
    public GameObject TrapObject;

    public override IEnumerator _OnSkillActive()
    {
        Debug.Log("Trap!");
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
