using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public float force = 1000;
    public float spinSpeed = 50;
    public float lifeTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(0, force));
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            transform.eulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * spinSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Kill()
    {
        lifeTime = 0;
    }
}
