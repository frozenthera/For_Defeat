using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSlash : Skill
{
    public GameObject SlashObject;
    public override IEnumerator _OnSkillActive()
    {
        GameObject go = Instantiate(SlashObject, origin.transform.position, Quaternion.identity);
        go.GetComponent<SlashObject>().origin = origin;
        go.GetComponent<SlashObject>().target = target;
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}