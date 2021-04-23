using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OIsoRigidbody : MonoBehaviour
{
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector3 newVel = velocity;
        newVel.y += velocity.z;

        transform.position += newVel * RealTimer.DeltaTime(gameObject);
    }
}
