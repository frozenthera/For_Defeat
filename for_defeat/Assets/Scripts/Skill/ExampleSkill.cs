using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSkill : Skill
{
    public override IEnumerator _OnSkillActive()
    {
        //do something
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
