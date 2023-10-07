using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 현재 각도
    Vector3 angle;
    // 마우스 감도
    public float sensitivity = 200f;

    // Start is called before the first frame update
    void Start()
    {
        // 시작할 때 현재 카메라의 각도를 적용한다
        angle.y = -Camera.main.transform.eulerAngles.x;
        angle.x = Camera.main.transform.eulerAngles.y;
        angle.z = Camera.main.transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 입력에 따라 카메라를 회전한다
        // 1. 사용자의 마우스 입력을 얻어온다
        // 마우스 좌우 입력을 받는다
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // 2. 방향을 연산한다
        // 이동 공식에 대입해서 각 속성 별로 회전 값을 누적한다
        angle.x += x * sensitivity * Time.deltaTime;
        angle.y += y * sensitivity * Time.deltaTime;

        // 3. 위에서 연산한 Axis 와 angle 을 사용해서 회전한다
        transform.eulerAngles = new Vector3(-angle.y, angle.x, angle.z);
    }
}
