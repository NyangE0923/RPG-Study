using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{

    //���� ������Ʈ cam ���� ����
    private GameObject cam;
    //parallaxȿ���� ���� ��
    [SerializeField] private float parallaxEffect;

    private float xPosition; //����� x ��ġ
    private float yPosition; //����� y ��ġ
    private float length; //����� ����

    void Start()
    {
        //Main Camera ������Ʈ�� ã���ش�.
        cam = GameObject.Find("Main Camera");
        //��� ��������Ʈ�� ���̸� �����´�.
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        //xPosition�� ��ġ�� ���� Main Camera�� x��ġ
        xPosition = transform.position.x;
        //yPosition�� ��ġ�� ���� Main Camera�� y��ġ
        yPosition = transform.position.y;
    }

    void Update()
    {
        // ����� �̵��Ÿ� ���, Parallax ȿ���� ���� ī�޶� �̵��� �ݴ� �������� ������ ����
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        //distanceToMove�� ���� ī�޶��� x��ġ�� parallaxEffect�� ���� ��
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // ����� �̵��Ÿ� ���, Parallax ȿ���� ���� ī�޶� �̵��� �ݴ� �������� ������ ����
        float distanceMoved2 = cam.transform.position.y * (1 - parallaxEffect);
        //distanceToMove�� ���� ī�޶��� y��ġ�� parallaxEffect�� ���� ��
        float distanceToMove2 = cam.transform.position.y * parallaxEffect;

        //ī�޶��� ��ġ = new Vector3�� x,y�� distanceToMove,distanceToMove2�� ���� ��
        transform.position = new Vector3(xPosition + distanceToMove, yPosition + distanceToMove2);

        // ����� ȭ���� ����� �ٽ� ȭ�� �������� �̵��ϵ��� ó���ϴ� ���ǹ�
        // ����� ȭ���� ���� �Ǵ� ���������� ����� �Ǹ� xPosition�� ��� ���̸� ���ϰų� ���� ȭ�� �������� �̵���Ų��.
        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}
