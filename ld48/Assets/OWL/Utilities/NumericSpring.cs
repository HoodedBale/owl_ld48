using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericSpring
{

    public static void Spring(ref float x, ref float vel, float xTarget, float timeDelta, float damp = 0.5f, float angularFreq = Mathf.PI)
    {
        float f = 1.0f + 2.0f * timeDelta * damp * angularFreq;
        float oo = angularFreq * angularFreq;
        float hoo = timeDelta * oo;
        float hhoo = timeDelta * hoo;
        float detInv = 1.0f / (f + hhoo);
        float detX = f * x + timeDelta * vel + hhoo * xTarget;
        float detV = vel + hoo * (xTarget - x);
        x = detX * detInv;
        vel = detV * detInv;
    }
}
