using UnityEngine;
using System.Collections;

//게임오브젝트를 만드는 클래스이다.
public class TrafficLightTimer 
{
    public static readonly float SIDEWALK_GREEN_TIME = 1.5f;
    public static readonly float DRIVEWAY_GREEN_TIME = 2.5f;


    public bool SidewalkOn { get; private set; }

    public TrafficLightTimer()
    {
        StartCoroutine(ControlLight());
    }


    private IEnumerator ControlLight()
    {
        float clock = Random.Range(0.0f, SIDEWALK_GREEN_TIME + DRIVEWAY_GREEN_TIME);

        while(true)
        {
            if ((clock >= 0) && (clock < SIDEWALK_GREEN_TIME))
            {
                SidewalkOn = true;
            }
            else if ((clock >= SIDEWALK_GREEN_TIME) && (clock <= SIDEWALK_GREEN_TIME + DRIVEWAY_GREEN_TIME))
            {
                SidewalkOn = false;
            }
            else throw new System.Exception();

            clock = (clock + GameMgr.COROUTINE_DELTATIME) % (SIDEWALK_GREEN_TIME + DRIVEWAY_GREEN_TIME);
            yield return new WaitForSeconds(GameMgr.COROUTINE_DELTATIME);
        }
    }

}
