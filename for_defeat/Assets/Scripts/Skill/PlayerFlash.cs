using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : Skill
{

    public override IEnumerator _OnSkillActive()
    {
        Debug.Log("Flash!");
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
