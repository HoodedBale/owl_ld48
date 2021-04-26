using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoshBoot : MonoBehaviour
{
    public float kickSpeed = 180;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Kick(float delay = 0.5f)
    {
        StartCoroutine(KickAnimation(delay));
    }

    IEnumerator KickAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        while(transform.eulerAngles.z < 90 && transform.eulerAngles.z >= -40)
        {
            transform.eulerAngles += new Vector3(0, 0, kickSpeed * Time.deltaTime);
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, 90);

        while(transform.eulerAngles.z > -40 && transform.eulerAngles.z <= 90)
        {
            transform.eulerAngles -= new Vector3(0, 0, kickSpeed * Time.deltaTime);
            yield return null;
        }

        transform.eulerAngles = Vector3.zero - new Vector3(0,0,40);
    }
}
