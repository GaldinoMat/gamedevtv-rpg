using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField]
    Transform target;
    public NavMeshAgent agent;
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            moveToCursor();
        }

        updateAnimator();
    }

    private void updateAnimator()
    {
        Vector3 velocity = agent.velocity;
        float localZ = transform.InverseTransformDirection(velocity).z;

        anim.SetFloat("forwardSpeed", localZ);
    }

    private void moveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        bool isHit = Physics.Raycast(ray, out hit);

        if (isHit)
        {
            agent.SetDestination(hit.point);
        }
    }
}
