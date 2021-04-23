using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimer : MonoBehaviour
{
    public RealTimer_SO timer;
    public float timeScale
    {
        get
        {
            if(timer != null)
                return timer.timeScale;
            return 1;
        }
    }

    public static float DeltaTime(GameObject go)
    {
        RealTimer timer = go.GetComponent<RealTimer>();
        if (timer) return timer.timeScale * Time.deltaTime;
        return Time.deltaTime;
    }
}
