using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMgr : MonoBehaviour {
    
    private static PrefabMgr _instance;
    public static PrefabMgr Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(PrefabMgr)) as PrefabMgr;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "MyClassContainer";
                    _instance = container.AddComponent(typeof(PrefabMgr)) as PrefabMgr;
                }
            }

            return _instance;
        }
    }
    /**************************GameObject Prefab*******************************/

    /// <summary>
    /// User Overlode of function "Instantiate".
    /// </summary>
    /// <returns>The instantiate.</returns>
    /// <param name="parent">Parent.</param>
    /// <param name="pos">Position.</param>
    /// <param name="rot">Only UDLR is alowed.</param>
    /// <param name="prefab">Prefab.</param>
    private GameObject Instantiate(GameObject prefab, Transform parent, Vector2 pos, Grid rot)
    {
        Quaternion quat;

        if (rot == Grid.up) quat = Quaternion.AngleAxis(0.0f, Vector3.forward);
        else if (rot == Grid.left) quat = Quaternion.AngleAxis(90.0f, Vector3.forward);
        else if (rot == Grid.down) quat = Quaternion.AngleAxis(180.0f, Vector3.forward);
        else if (rot == Grid.left) quat = Quaternion.AngleAxis(270.0f, Vector3.forward);
        else throw new System.Exception();

        GameObject obj = Instantiate(prefab, pos, quat, parent);
        return obj;
    }

    public GameObject drivewayTrafficLight_Prefab;
    public void Instantiate_DrivewayTrafficLight(Transform parent, Vector2 pos, Grid rot, TrafficLightTimer trafficLight)
    {
        GameObject obj = Instantiate(drivewayTrafficLight_Prefab, parent, pos, rot);

        DrivewayTrafficLight comp = obj.AddComponent<DrivewayTrafficLight>();

        comp.SetTrafficLight(trafficLight);
        comp.SetComponent();
        StartCoroutine(comp.SetSpriteRenderer());
    }
    public class DrivewayTrafficLight : MonoBehaviour
    {
        private TrafficLightTimer trafficLight;

        /********필요한 컴포넌트들**********/
        private SpriteRenderer spriteRenderer;
        /******************************/

        public void SetTrafficLight(TrafficLightTimer trafficLight)
        {
            this.trafficLight = trafficLight;
        }
        public void SetComponent()
        {
            spriteRenderer = this.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) throw new System.Exception();
        }
        public IEnumerator SetSpriteRenderer()
        {
            while (true)
            {
                //초록불일때
                if (!trafficLight.SidewalkOn)
                {
                    ///
                }
                //빨간불일때
                else
                {
                    ///
                }
                yield return new WaitForSeconds(GameMgr.COROUTINE_DELTATIME);
            }
        }
    }

    public GameObject sidewalkTrafficLight_Prefab;
    public void Instantiate_SidewalkTrafficLight(Transform parent, Vector2 pos, Grid rot, TrafficLightTimer trafficLight)
    {
        GameObject obj = Instantiate(sidewalkTrafficLight_Prefab, parent, pos, rot);

        SidewalkTrafficLight comp = obj.AddComponent<SidewalkTrafficLight>();



        comp.SetTrafficLight(trafficLight);
        comp.SetComponent();
        StartCoroutine(comp.SetSpriteRenderer());
    }
    public class SidewalkTrafficLight : MonoBehaviour
    {
        private TrafficLightTimer trafficLight;

        /********필요한 컴포넌트들**********/
        private SpriteRenderer spriteRenderer;
        /******************************/
        public void SetTrafficLight(TrafficLightTimer trafficLight)
        {
            this.trafficLight = trafficLight;
        }
        public void SetComponent()
        {
            spriteRenderer = this.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) throw new System.Exception();
        }
        public IEnumerator SetSpriteRenderer()
        {
            while (true)
            {
                //초록불일때
                if (trafficLight.SidewalkOn)
                {
                    ///
                }
                //빨간불일때
                else
                {
                    ///
                }
                yield return new WaitForSeconds(GameMgr.COROUTINE_DELTATIME);
            }
        }
    }
}
