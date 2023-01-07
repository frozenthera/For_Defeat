using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : Skill
{   private GameObject RangeIndicator;
    private float flashRadius;
    private PlayerController player;

    private void Start()
    {
        player = GameManager.Instance.player;
        RangeIndicator = player.RangeIndicator;
        flashRadius = player.FlashRadius;
    }

    public override IEnumerator OnSkillActive()
    {
        RangeIndicator.gameObject.SetActive(false);
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = origin.transform.position.z;
        if((origin.transform.position - targetPosition).magnitude <= flashRadius) 
        {
            yield return StartCoroutine(EBDelay());
            origin.transform.position = targetPosition;
            yield return StartCoroutine(_OnSkillActive());
            yield return StartCoroutine(EADelay());
        }
        else
        {
            yield return StartCoroutine(EBDelay());
            origin.transform.position += (targetPosition - origin.transform.position).normalized * flashRadius;
            yield return StartCoroutine(_OnSkillActive());
            yield return StartCoroutine(EADelay());
        }
    }

    public override IEnumerator _OnSkillActive()
    {
        StartCoroutine(GameManager.Instance.player.EFlashCD());
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}