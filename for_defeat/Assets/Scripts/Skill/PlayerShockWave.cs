using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShockWave : Skill
{
   public GameObject ShockWaveObject;

    public override IEnumerator _OnSkillActive()
    {
        Debug.Log("ShockWave!");
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
