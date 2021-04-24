using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringAppearance : MonoBehaviour
{
    public float damp = 0.2f;
    public float speed = 8;
    public Vector3 startSize;
    public Vector3 endSize;

    Vector3 m_vel;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = startSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        NumericSpring.Spring(ref scale.x, ref m_vel.x, endSize.x, Time.deltaTime, damp, speed * Mathf.PI);
        NumericSpring.Spring(ref scale.y, ref m_vel.y, endSize.y, Time.deltaTime, damp, speed * Mathf.PI);
        NumericSpring.Spring(ref scale.z, ref m_vel.z, endSize.z, Time.deltaTime, damp, speed * Mathf.PI);
        transform.localScale = scale;
    }

    public void ResetUI()
    {
        transform.localScale = startSize;
    }
}
