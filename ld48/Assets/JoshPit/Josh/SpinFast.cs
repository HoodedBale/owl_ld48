using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinFast : MonoBehaviour
{
    public float spinSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RealTimer>().timer.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(0, 0, 1) * spinSpeed * RealTimer.DeltaTime(gameObject);
    }
}
