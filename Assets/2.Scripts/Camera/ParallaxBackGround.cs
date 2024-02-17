using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{

    //게임 오브젝트 cam 변수 생성
    private GameObject cam;
    //parallax효과에 대한 값
    [SerializeField] private float parallaxEffect;

    private float xPosition; //배경의 x 위치
    private float yPosition; //배경의 y 위치
    private float length; //배경의 길이

    void Start()
    {
        //Main Camera 오브젝트를 찾아준다.
        cam = GameObject.Find("Main Camera");
        //배경 스프라이트의 길이를 가져온다.
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        //xPosition의 위치는 현재 Main Camera의 x위치
        xPosition = transform.position.x;
        //yPosition의 위치는 현재 Main Camera의 y위치
        yPosition = transform.position.y;
    }

    void Update()
    {
        // 배경의 이동거리 계산, Parallax 효과에 의해 카메라 이동의 반대 방향으로 움직임 구현
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        //distanceToMove는 현재 카메라의 x위치에 parallaxEffect를 곱한 값
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // 배경의 이동거리 계산, Parallax 효과에 의해 카메라 이동의 반대 방향으로 움직임 구현
        float distanceMoved2 = cam.transform.position.y * (1 - parallaxEffect);
        //distanceToMove는 현재 카메라의 y위치에 parallaxEffect를 곱한 값
        float distanceToMove2 = cam.transform.position.y * parallaxEffect;

        //카메라의 위치 = new Vector3의 x,y에 distanceToMove,distanceToMove2를 더한 값
        transform.position = new Vector3(xPosition + distanceToMove, yPosition + distanceToMove2);

        // 배경이 화면을 벗어나면 다시 화면 안쪽으로 이동하도록 처리하는 조건문
        // 배경이 화면을 왼쪽 또는 오른쪽으로 벗어나게 되면 xPosition에 배경 길이를 더하거나 빼서 화면 안쪽으로 이동시킨다.
        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}
