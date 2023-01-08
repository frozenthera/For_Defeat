using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : PlayerSkill
{   private GameObject RangeIndicator;
    private float flashRadius;
    private float calculatedFlashRadius;
    private PlayerController player;
    private HeroBehaviour hero;

    private void Start()
    {
        player = GameManager.Instance.player;
        hero = GameManager.Instance.hero;
        RangeIndicator = player.RangeIndicator;
        flashRadius = player.FlashRadius;
    }

    public override IEnumerator OnSkillActive(int skillIdx)
    {
        calculatedFlashRadius = flashRadius * (((int)(player.CurAngerGauge/333))+1);
        RangeIndicator.gameObject.SetActive(false);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(player.IsBerserker)
        {
            targetPosition = hero.transform.position;
        }
        targetPosition.z = origin.transform.position.z;
        if((origin.transform.position - targetPosition).magnitude <= calculatedFlashRadius) 
        {
            yield return StartCoroutine(EBDelay());
            origin.transform.position = targetPosition;
            yield return StartCoroutine(_OnSkillActive());
            yield return StartCoroutine(EADelay());
        }
        else
        {
            yield return StartCoroutine(EBDelay());
            origin.transform.position += (targetPosition - origin.transform.position).normalized * calculatedFlashRadius;
            yield return StartCoroutine(_OnSkillActive());
            yield return StartCoroutine(EADelay());
        }
    }

    public override IEnumerator _OnSkillActive()
    {
        StartCoroutine(GameManager.Instance.player.EFlashCD());
        yield return null;
    }
}