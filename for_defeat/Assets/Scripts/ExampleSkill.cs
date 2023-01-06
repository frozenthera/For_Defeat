using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSkill : Skill
{
    public override void OnSkillActive()
    {
        //do something
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
