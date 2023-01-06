using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerErosion : Skill
{
   public GameObject ErosionObject;

    public override IEnumerator _OnSkillActive()
    {
        Debug.Log("Erosion!");
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
