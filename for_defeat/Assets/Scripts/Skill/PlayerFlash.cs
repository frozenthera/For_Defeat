using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : PlayerSkill
{
    [SerializeField] private GameObject RangeIndicatorPrefab;
    private GameObject RangeIndicator;
    [SerializeField] private float FlashRadius;

    private void Start()
    {
        RangeIndicator = Instantiate(RangeIndicatorPrefab, Vector3.zero, Quaternion.identity);
        RangeIndicator.transform.localScale = Vector3.forward + new Vector3(1,1,0) * FlashRadius * 10f;
        RangeIndicator.gameObject.SetActive(false);
    }

    public override IEnumerator OnSkillActive(int skillIdx)
    {
        RangeIndicator.transform.position = origin.transform.position;
        RangeIndicator.transform.parent = origin.transform;
        RangeIndicator.gameObject.SetActive(true);
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
        if(Input.GetMouseButtonDown(0))
        {
            RangeIndicator.gameObject.SetActive(false);
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = origin.transform.position.z;
            if((origin.transform.position - targetPosition).magnitude <= FlashRadius) 
            {
                yield return StartCoroutine(EBDelay());
                origin.transform.position = targetPosition;
                yield return StartCoroutine(_OnSkillActive());
                yield return StartCoroutine(EADelay());
                StartCoroutine(GameManager.Instance.player.ECoolDownDic[(PlayerController.EPlayerSkill)skillIdx]);
            }
            else
            {
                yield return StartCoroutine(EBDelay());
                origin.transform.position += (targetPosition - origin.transform.position).normalized * FlashRadius;
                yield return StartCoroutine(_OnSkillActive());
                yield return StartCoroutine(EADelay());
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            RangeIndicator.gameObject.SetActive(false);
        }
    }

    public override IEnumerator _OnSkillActive()
    {
        StartCoroutine(GameManager.Instance.player.EFlashCD());
        yield return null;
    }
}