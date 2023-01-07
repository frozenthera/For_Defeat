using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveObject : MonoBehaviour
{
    public float damage;
    public float knuckBackSec;
    public Vector3 knuckBackVec;
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log(coll.name);
        if(coll.transform.CompareTag("Hero"))
        {
            coll.GetComponent<HeroBehaviour>().GetDamage(damage);
            coll.GetComponent<HeroBehaviour>().UpdateState(HeroBehaviour.HeroState.KnuckBack, knuckBackVec, knuckBackSec);
        }
    }

    private void Start()
    {
        Destroy(gameObject, .5f);
    }
}
