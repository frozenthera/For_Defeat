using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerErosion : Skill
{
   public GameObject ErosionObject;
   [SerializeField] private float DOTLastingTime; 
   [SerializeField] private float DOTDamagePerSec;
   [SerializeField] private float Radius;
   [SerializeField] private float damage;

    public override IEnumerator _OnSkillActive()
    {
        //TODO : apply damage when first casted
        
        ErosionObject EO = Instantiate(ErosionObject, origin.transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<ErosionObject>();
        EO.lastTime = DOTLastingTime;
        EO.damagePerSec = DOTDamagePerSec;
        EO.damage = damage;
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
