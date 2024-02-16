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
    private float length; //����� ����

    void Start()
    {
        //Main Camera ������Ʈ�� ã���ش�.
        cam = GameObject.Find("Main Camera");
        //��� ��������Ʈ�� ���̸� �����´�.
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        //xPosition�� ��ġ�� ���� Main Camera�� x��ġ
        xPosition = transform.position.x;
    }

    void Update()
    {
        // ����� �̵��Ÿ� ���, Parallax ȿ���� ���� ī�޶� �̵��� �ݴ� �������� ������ ����
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        //distanceToMove�� ���� ī�޶��� x��ġ�� parallaxEffect�� ���� ��
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        //x�� xPosition�� ������ �����ǰ�, y�� ���� ī�޶��� ��ġ�� �״�� �������� ������ ����� �������� �̵��ϰ� �ȴ�.
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        // ����� ȭ���� ����� �ٽ� ȭ�� �������� �̵��ϵ��� ó���ϴ� ���ǹ�
        // ����� ȭ���� ���� �Ǵ� ���������� ����� �Ǹ� xPosition�� ��� ���̸� ���ϰų� ���� ȭ�� �������� �̵���Ų��.
        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}
