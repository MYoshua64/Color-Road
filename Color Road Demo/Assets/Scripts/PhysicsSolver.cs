using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSolver : MonoBehaviour
{
    public static Vector3 InitialVelocity(Vector3 startPos, Vector3 targetPos, float time)
    {
        //x_t = x_0 + v_0 * t + gt^2/2
        //2x_t = 2x_0 + 2v_0 * t + gt^2
        //2v_0t = 2x_t - 2x_0 - gt^2
        //v_0 = (2x_t - 2x_0 - gt^2) / 2t

        return (2 * targetPos - 2 * startPos - Physics.gravity * time * time) / (2 * time);
    }
}
