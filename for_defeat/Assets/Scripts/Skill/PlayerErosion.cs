using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerErosion : PlayerSkill
{
    public GameObject ErosionObjectPrefab;
    private GameObject ErosionEffect;
    [SerializeField] private float RadiusMultiplier;
    [SerializeField] private float damage;
    [SerializeField] private float fadeOutTime = 1f;

    private void Start()
    {
        ErosionEffect = Instantiate(ErosionObjectPrefab, Vector3.zero, Quaternion.identity);
        ErosionEffect.SetActive(false);
    }

    public override IEnumerator _OnSkillActive()
    {
        int AngerStep = (int)(GameManager.Instance.player.CurAngerGauge / 333) + 1;

        if((origin.transform.position-GameManager.Instance.hero.transform.position).magnitude < RadiusMultiplier * (AngerStep + 1))
            GameManager.Instance.hero.GetDamage(damage);
        ErosionEffect.transform.localScale = Vector3.forward + new Vector3(10, 10, 0) * RadiusMultiplier * (AngerStep + 1);
        StartCoroutine(EErosionEffect());
        yield return null;
    }

    private IEnumerator EErosionEffect()
    {
        float curFadeOut = fadeOutTime;
        Material mat = ErosionEffect.GetComponent<SpriteRenderer>().material;
        ErosionEffect.transform.position = GameManager.Instance.player.transform.position;
        ErosionEffect.SetActive(true);
        while(curFadeOut >= 0)
        {
            mat.color = new Color(mat.color.r,mat.color.g, mat.color.b, curFadeOut);
            curFadeOut -= Time.deltaTime;
            yield return null;
        }
        ErosionEffect.SetActive(false);
        mat.color = new Color(mat.color.r,mat.color.g, mat.color.b, 1f);
    }
}