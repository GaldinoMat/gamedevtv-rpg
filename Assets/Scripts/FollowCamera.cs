using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    private void LateUpdate()
    {
        gameObject.transform.position = target.position;
    }
}
