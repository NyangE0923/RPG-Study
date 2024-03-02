using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private Blackhole_Skill_Controller blackHole;
    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            //���� myHotKey�� ������ Blackhole_Skill_Controller Ŭ������ AddEnemyToList�޼��带 ȣ���ϰ�
            //�̶� �ش� �޼���� �Ű������� Transform�� ����ϹǷ� Transform������ myEnemy�� ����Ѵ�.
            blackHole.AddEnemyToList(myEnemy);
            //Text�� ����� Sprite�� ����, ���İ��� 0���� ������ �����ϰ� �����.
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
