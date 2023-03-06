using System;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class AllIn1LookAt : MonoBehaviour
    {
        //Otherwise we just update on Start
        [SerializeField] private bool updateEveryFrame;
        
        [Space, Header("Choose Target")]
        [SerializeField] private bool targetIsMainCamera;
        [SerializeField] private Transform target;
        
        private enum FaceDirection {
            Forward,
            Up,
            Right
        };
        [Space, Header("Look At Direction")]
        [SerializeField] private FaceDirection faceDirection;
        [SerializeField] private bool negateDirection;

        private void Start()
        {
            if(targetIsMainCamera)
            {
                if(!(Camera.main is null)) target = Camera.main.transform;
                if(target == null)
                {
                    Debug.LogError("No main camera was found, AllIn1LookAt component of " + gameObject.name + " will now be destroyed. Please double check your setup");
                    Destroy(this);
                }
            }
            else
            {
                if(target == null)
                {
                    Debug.LogError("No target was assigned, AllIn1LookAt component of " + gameObject.name + " will now be destroyed. Please double check your setup");
                    Destroy(this);
                }
            }
            
            if(!updateEveryFrame) LookAtCompute();
        }

        private void Update()
        {
            if(updateEveryFrame) LookAtCompute();
        }

        private void LookAtCompute()
        {
            Vector3 lookAtVector = (target.position - transform.position).normalized;
            if(negateDirection) lookAtVector = -lookAtVector;
            switch(faceDirection)
            {
                case FaceDirection.Forward:
                    transform.forward = lookAtVector;
                    break;
                case FaceDirection.Up:
                    transform.up = lookAtVector;
                    break;
                case FaceDirection.Right:
                    transform.right = lookAtVector;
                    break;
            }
        }
    }
}