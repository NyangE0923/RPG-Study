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
            //만약 myHotKey를 누르면 Blackhole_Skill_Controller 클래스의 AddEnemyToList메서드를 호출하고
            //이때 해당 메서드는 매개변수인 Transform을 사용하므로 Transform변수인 myEnemy를 사용한다.
            blackHole.AddEnemyToList(myEnemy);
            //Text의 색상과 Sprite의 색상, 알파값을 0으로 조정해 투명하게 만든다.
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
