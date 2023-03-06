using System;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    [ExecuteInEditMode]
    public class AllIn1VfxFakeLightDirSetter : MonoBehaviour
    {
        [Header("This script gets executed in edit mode too")]
        [SerializeField] private bool setOnAwake = true;
        [SerializeField] private bool setOnUpdate = false;
        
        [Space, Header("If target is null we'll use this object as target")] 
        [Header("Direction is target's forward vector")]
        [SerializeField] private Transform target;

        private int lightDirId = 0;
        
        private void Awake()
        {
            if(setOnAwake) SetGlobalFakeLightDir();
        }
        
        private void Update()
        {
            if(setOnUpdate) SetGlobalFakeLightDir();
        }

        private void OnValidate()
        {
            SetGlobalFakeLightDir();
        }

        public void SetGlobalFakeLightDir()
        {
            if(lightDirId == 0) lightDirId = Shader.PropertyToID("_All1VfxLightDir");
            if(target == null) target = transform;
            Shader.SetGlobalVector(lightDirId, target.forward.normalized);
        }

        public void SetNewFakeLightDir(Vector3 newFakeLightDir)
        {
            if(lightDirId == 0) lightDirId = Shader.PropertyToID("_All1VfxLightDir");
            Shader.SetGlobalVector(lightDirId, newFakeLightDir.normalized);
        }
        
        public void SetNewTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        public void SetOnUpdateBool(bool newSetOnUpdateValue)
        {
            setOnUpdate = newSetOnUpdateValue;
        }
    }
}