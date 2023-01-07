using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErosionObject : MonoBehaviour
{
    //1초당 받는 데미지
    public float damagePerSec;
    //장판 지속시간
    public float lastTime;
    private void Start()
    {
        StartCoroutine(ExtinguishTimer());
    }

    private void OnTriggerStay(Collider coll)
    {
        if(coll.transform.CompareTag("Hero"))
        {
            // Debug.Log("Getting DOT");
            HeroBehaviour HB = coll.transform.GetComponent<HeroBehaviour>();
            HB.GetDamage(damagePerSec * Time.deltaTime);
        }
    }

    private IEnumerator ExtinguishTimer()
    {
        float curTime = lastTime;
        while(curTime >= 0)
        {
            curTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    
}
