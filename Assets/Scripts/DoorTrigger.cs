using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // press E to toggle
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
        }
    }
}
