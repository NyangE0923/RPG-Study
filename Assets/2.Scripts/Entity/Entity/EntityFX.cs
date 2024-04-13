using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float FlashDuration; //�ڷ�ƾ Ÿ�� ����Ʈ ���ӽð�
    private Material originalMat;

    [Header("Ailment colors")]
    //�� �Ӽ��� ���� ����ȭ�� private Color���� ����
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
        //��������Ʈ ���͸����� hitMat���� ����
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color = Color.white;
        //yield return (0.2���� ��ȯ)
        yield return new WaitForSeconds(FlashDuration);
        sr.color = currentColor;
        //��������Ʈ ���͸����� originalMat���� ����
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
        //������ �ֱ�� ������ �޼ҵ带 ȣ���ϴ� �Լ�
        //��� ȣ���� �� 0.3�ʸ��� �ݺ� ȣ���ϸ�, Invoke�� ���ؼ� �Ű����� _seconds��ŭ ������ CancelColorChange �޼ҵ带 ȣ���Ѵ�.
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
        //������ �ֱ�� ������ �޼ҵ带 ȣ���ϴ� �Լ�
        //��� ȣ���� �� 0.3�ʸ��� �ݺ� ȣ���ϸ�, Invoke�� ���ؼ� �Ű����� _seconds��ŭ ������ CancelColorChange �޼ҵ带 ȣ���Ѵ�.
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        //��������Ʈ�� ������ igniteColor�� �ƴ϶�� igniteColor�迭�� 0�� �ش��ϴ� ������ �ǰ�
        //igniteColor��� igniteColor�迭�� 1�� �ش��ϴ� ������ �ȴ�.
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
