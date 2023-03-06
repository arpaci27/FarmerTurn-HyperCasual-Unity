#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    [CustomEditor(typeof(AllIn1VfxComponent)), CanEditMultipleObjects]
    public class AllIn1VfxEditor : UnityEditor.Editor
    {
        private GUIStyle smallBoldLabel;

        public override void OnInspectorGUI()
        {
            Texture2D imageInspector = Resources.Load<Texture2D>("CustomEditorTransparent");
            if(imageInspector)
            {
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(32));
                GUI.DrawTexture(rect, imageInspector, ScaleMode.ScaleToFit, true);
            }

            AllIn1VfxComponent myScript = (AllIn1VfxComponent) target;

            if(GUILayout.Button("Deactivate All Effects"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).ClearAllKeywords();
            }

            if(GUILayout.Button("New Clean Material"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).TryCreateNew();
            }

            if(GUILayout.Button("Create New Material With Same Properties (SEE DOC)"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).MakeCopy();
            }

            if(GUILayout.Button("Save Material To Folder (SEE DOC)"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).SaveMaterial();
            }

            if(GUILayout.Button("Apply Material To All Children"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).ApplyMaterialToHierarchy();
            }

            if(GUILayout.Button("Render Material To Image"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).RenderToImage();
            }

            CheckIfShowParticleSystemHelperUI();

            EditorGUILayout.Space();
            DrawLine(Color.grey, 1, 3);

            if(GUILayout.Button("Remove Component"))
            {
                for(int i = targets.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(targets[i] as AllIn1VfxComponent);
                    (targets[i] as AllIn1VfxComponent).SetSceneDirty();
                }
            }

            if(GUILayout.Button("REMOVE COMPONENT AND MATERIAL"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).CleanMaterial();
                for(int i = targets.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(targets[i] as AllIn1VfxComponent);
                    (targets[i] as AllIn1VfxComponent).SetSceneDirty();
                }
            }
        }

        private void CheckIfShowParticleSystemHelperUI()
        {
            if(Selection.activeGameObject?.GetComponent<ParticleSystem>() == null) return;
            AllIn1ParticleHelperComponent all1VfxPsHelper = Selection.activeGameObject.GetComponent<AllIn1ParticleHelperComponent>();
            if(all1VfxPsHelper != null) return;
            DrawLine(Color.grey, 1, 3);
            EditorGUILayout.Space();
            if(GUILayout.Button("Add Particle System Helper"))
            {
                for(int i = 0; i < targets.Length; i++) ((AllIn1VfxComponent) targets[i]).AddHelperAndPlaceUnderAll1VfxMainComponent();
            }
        }

        private void DrawLine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += (padding / 2);
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}
#endif