using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSlash : Skill
{
    public GameObject SlashObject;
    public override IEnumerator _OnSkillActive()
    {
        Instantiate(SlashObject, origin.transform.position, Quaternion.identity);
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}