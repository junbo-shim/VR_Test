using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GazePointer : MonoBehaviour
{
    public Video360Play video360Play = default;

    public Transform uiCanvas = default;
    public Image gazeImg = default;

    private Vector3 defaultScale = Vector3.one;
    public float uiScale = 1.0f;

    private bool isHitObj = false;
    private GameObject prevHitObj = default;
    private GameObject currentHitObj = default;
    float currentGazeTime = 0f;
    public float gazeChargeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        defaultScale = uiCanvas.localScale;
        currentGazeTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 캔버스 오브젝트의 스케일을 거리에 따라 조절한다
        // 1. 카메라를 기준으로 전방 방향의 좌표를 구한다
        Vector3 direction = transform.TransformPoint(Vector3.forward);

        // 2. 카메라를 기준으로 전방의 레이를 설정한다
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo = default;

        // 3. 레이에 부딪힌 경우에는 거리 값을 이용해 uiCanvas 의 크기를 조절한다
        if (Physics.Raycast(ray, out hitInfo)) 
        {
            uiCanvas.localScale = defaultScale * uiScale * hitInfo.distance;
            uiCanvas.position = transform.forward * hitInfo.distance;

            if (hitInfo.transform.tag == "GazeObj") 
            {
                isHitObj = true;
            }
            currentHitObj = hitInfo.transform.gameObject;
        }
        // 4. 아무 것도 부딪히지 않으면 기본 스케일 값으로 uiCanvas 크기를 조절한다
        else 
        {
            uiCanvas.localScale = defaultScale * uiScale;
            uiCanvas.position = transform.position + direction;
        }

        // 5. uiCanvas 가 항상 카메라 오브젝트를 바라보게 한다
        uiCanvas.forward = transform.forward * (-1.0f);

        // GazeObj 에 레이가 닿았을 때 실행
        if (isHitObj) 
        {
            // 현재 프레임의 오브젝트가 이전 프레임의 오브젝트에서 머물러 있는 경우
            if (currentHitObj == prevHitObj) 
            {
                // 인터랙션이 발생해야 하는 오브젝트에서 시선이 고정되어 있다면 시간을 증가시킨다
                currentGazeTime += Time.deltaTime;
            }
            // 현재 프레임의 오브젝트가 이전 프레임의 오브젝트에서 벗어난 경우
            else
            {
                // 이전 프레임의 영상 정보를 업데이트한다
                prevHitObj = currentHitObj;
            }

            HitObjectChecker(currentHitObj, true);
        }
        else 
        {
            if (prevHitObj != null && prevHitObj != default) 
            {
                HitObjectChecker(prevHitObj, false);
                prevHitObj = default;
            }
            // 시선이 벗어났거나 GazeObj 가 아닌 오브젝트를 바라보는 경우
            currentGazeTime = 0f;
        }

        // 시선이 머문 시간을 0과 최댓값 사이로 한다
        currentGazeTime = Mathf.Clamp(currentGazeTime, 0f, gazeChargeTime);
        // UI Img 의 fill amount 를 업데이트 한다
        gazeImg.fillAmount = currentGazeTime / gazeChargeTime;

        // GazePointer 의 게이지를 한 프레임만큼 올린 다음 현재 프레임에 사용된 변수들을 초기화한다
        isHitObj = false;
        currentHitObj = default;
    }

    //! 히트된 오브젝트 타입별로 작동 방식을 구분한다
    private void HitObjectChecker(GameObject hitObject, bool isActive) 
    {
        // hit 가 비디오 플레이어 컴포넌트를 갖고 있는지 확인한다
        if (hitObject.GetComponent<VideoPlayer>()) 
        {
            if (isActive) 
            {
                hitObject.GetComponent<VideoFrame>().CheckVideoFrame(true);
            }
            else
            {
                hitObject.GetComponent<VideoFrame>().CheckVideoFrame(false);
            }

            // 정해진 시간이 되면 360 스피어에 특정 클립 번호를 전달해 플레이 한다
            if (currentGazeTime / gazeChargeTime >= 1.0f) 
            {
                // 비디오 플레이어가 없는 Mesh collider 오브젝트
                // 이름에 따라 이전/다음 영상으로 재생한다
                if (hitObject.name.Contains("Right")) 
                {
                    video360Play.SwapVideoClip(true);
                }
                else if (hitObject.name.Contains("Left")) 
                {
                    video360Play.SwapVideoClip(false);
                }
                else 
                {
                    // ?
                    video360Play.SetVideoPlay(currentHitObj.transform.GetSiblingIndex());
                }
                currentGazeTime = 0f;
            }

            #region Legacy
            //if(gazeImg.fillAmount >= 1.0f) 
            //{
            //    video360Play.SetVideoPlay(currentHitObj.transform.GetSiblingIndex());
            //}
            #endregion
        }
    }
}
