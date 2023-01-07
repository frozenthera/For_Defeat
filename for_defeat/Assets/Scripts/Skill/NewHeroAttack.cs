using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHeroAttack : HeroSkill
{
    private HeroBehaviour hero;

    private void Start()
    {

    }
    public override IEnumerator _OnSkillActive()
    {
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
