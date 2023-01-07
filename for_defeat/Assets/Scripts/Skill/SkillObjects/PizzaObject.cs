using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaObject : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.transform.CompareTag("Hero"))
        {
            coll.GetComponent<HeroBehaviour>().GetDamage(damage);
        }
    }

    private void Start()
    {
        Destroy(gameObject, .5f);
    }
}
