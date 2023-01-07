using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : Skill
{
    [SerializeField] private GameObject RangeIndicatorPrefab;
    private GameObject RangeIndicator;
    [SerializeField] private float FlashRadius;

    private void Start()
    {
        RangeIndicator = Instantiate(RangeIndicatorPrefab, Vector3.zero, Quaternion.identity);
        RangeIndicator.gameObject.SetActive(false);
    }

    public override IEnumerator _OnSkillActive()
    {
        RangeIndicator.transform.position = origin.transform.position;
        RangeIndicator.transform.parent = origin.transform;
        RangeIndicator.gameObject.SetActive(true);
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
        //점멸 사용
        if(Input.GetMouseButtonDown(0))
        {
            RangeIndicator.gameObject.SetActive(false);
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = origin.transform.position.z;
            if((origin.transform.position - targetPosition).magnitude <= FlashRadius) 
            {
                origin.transform.position = targetPosition;
                StartCoroutine(GameManager.Instance.player.EFlashCD());
            }
        }
        //행동 취소
        else if(Input.GetMouseButtonDown(1))
        {
            RangeIndicator.gameObject.SetActive(false);
        }
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}