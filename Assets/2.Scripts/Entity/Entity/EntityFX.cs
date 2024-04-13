using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float FlashDuration; //코루틴 타격 이펙트 지속시간
    private Material originalMat;

    [Header("Ailment colors")]
    //각 속성에 대한 직렬화된 private Color변수 정의
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        //스프라이트 머터리얼을 hitMat으로 변경
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color = Color.white;
        //yield return (0.2초후 반환)
        yield return new WaitForSeconds(FlashDuration);
        sr.color = currentColor;
        //스프라이트 머터리얼을 originalMat으로 변경
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }


    public void IgniteFxFor(float _seconds)
    {
        //일정한 주기로 지정된 메소드를 호출하는 함수
        //즉시 호출한 뒤 0.3초마다 반복 호출하며, Invoke에 의해서 매개변수 _seconds만큼 지난후 CancelColorChange 메소드를 호출한다.
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        //일정한 주기로 지정된 메소드를 호출하는 함수
        //즉시 호출한 뒤 0.3초마다 반복 호출하며, Invoke에 의해서 매개변수 _seconds만큼 지난후 CancelColorChange 메소드를 호출한다.
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        //스프라이트의 색상이 igniteColor이 아니라면 igniteColor배열의 0에 해당하는 색상이 되고
        //igniteColor라면 igniteColor배열의 1에 해당하는 색상이 된다.
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    private void ChillColorFx()
    {
        if(sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

}
