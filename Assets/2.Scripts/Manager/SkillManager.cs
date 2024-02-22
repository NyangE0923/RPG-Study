using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash {  get; private set; }
    public Counter_Skill counter { get; private set; }
    public Clone_Skill clone { get; private set; }

    private void Awake()
    {
        //SkillManger가 null이 아니라면 파괴한다.
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        counter = GetComponent<Counter_Skill>();
        clone = GetComponent<Clone_Skill>();
    }
}
