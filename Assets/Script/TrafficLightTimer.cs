using UnityEngine;
using System.Collections;
using System;

//게임오브젝트를 만드는 클래스이다.
public class TrafficLightTimer : MonoBehaviour
{
    public static readonly float SIDEWALK_GREEN_TIME = 1.5f;
    public static readonly float DRIVEWAY_GREEN_TIME = 2.5f;

    /// <summary> 
    /// Sidewalk에서 통행 가능할 때 true, Driveway에서 통행 가능할 때 false를 인자로 사용하여 호출
    /// </summary>
    public event Action<bool> OnLightChanged;

    public bool SidewalkOn { get; private set; } 

    public TrafficLightTimer()
    {
        StartCoroutine(ControlLight());
    }

    private IEnumerator ControlLight()
    {
        SidewalkOn = false;

        float initTime = UnityEngine.Random.Range(0.0f, SIDEWALK_GREEN_TIME + DRIVEWAY_GREEN_TIME);
        yield return new WaitForSeconds(initTime);

        WaitForSeconds wSidewalk = new WaitForSeconds(SIDEWALK_GREEN_TIME);
        WaitForSeconds wDriveway = new WaitForSeconds(DRIVEWAY_GREEN_TIME);
        while(true)
        {
            SidewalkOn = true;
            if (OnLightChanged != null) OnLightChanged(true);
            yield return wSidewalk;

            SidewalkOn = false;
            if (OnLightChanged != null) OnLightChanged(false);
            yield return wDriveway;
        }
    }
}
