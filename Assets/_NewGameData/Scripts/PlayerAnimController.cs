using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInteractController playerInteractController;
    
    void Update()
    {
        if (playerInteractController.stackParent.childCount!= 0){
            animator.SetBool("isCarrying", true);
        }
        else
        {
            animator.SetBool("isCarrying", false);
        }
    }
}
