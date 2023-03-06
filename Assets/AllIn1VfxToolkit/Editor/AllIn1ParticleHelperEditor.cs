#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    [CustomEditor(typeof(AllIn1ParticleHelperComponent)), CanEditMultipleObjects]
    public class AllIn1ParticleHelperEditor : Editor
    {
        private GUIStyle smallRedBoldLabel, smallBoldLabel, headerBoldLabel, propertiesStyle;

        private AllIn1VfxComponent all1Vfx;
        private ParticleSystem ps;
        private ParticleSystem.MainModule mainModule;
        private Color targetColor = Color.white;
        private int gradientColorKeyIndex = 0;
        private int helperNum;
        private SerializedProperty startColorProperty;

        private void OnEnable()
        {
            startColorProperty = serializedObject.FindProperty("startColor");
        }

        public override void OnInspectorGUI()
        {
            smallRedBoldLabel = new GUIStyle(EditorStyles.boldLabel) { normal = { textColor = new Color(0.58f, 0f, 0f) } };
            smallBoldLabel = new GUIStyle(EditorStyles.boldLabel);
            headerBoldLabel = new GUIStyle(EditorStyles.boldLabel) { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            propertiesStyle = new GUIStyle(EditorStyles.helpBox) { margin = new RectOffset(0, 0, 0, 0) };
            helperNum = 1;

            if(Selection.activeGameObject != null) ps = Selection.activeGameObject.GetComponent<ParticleSystem>();
            if(ps == null)
            {
                GUILayout.Label("Please add a Particle System to this Game Object", smallRedBoldLabel);
                EditorGUILayout.Space();
                if(GUILayout.Button("Add New Particle System")) Selection.activeGameObject.AddComponent<ParticleSystem>();
                return;
            }

            if(Selection.activeGameObject != null) all1Vfx = Selection.activeGameObject.GetComponent<AllIn1VfxComponent>();
            if(all1Vfx == null)
            {
                if(GUILayout.Button("Add All In 1 Vfx Component"))
                {
                    AllIn1VfxComponent t = Selection.activeGameObject.AddComponent<AllIn1VfxComponent>();
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(t);
                }

                DrawLine(Color.grey, 1, 3);
            }

            mainModule = ps.main;

            EditorGUILayout.LabelField("Generic Helpers", headerBoldLabel, GUILayout.Height(20));
            HierarchyHelpers();
            ColorChangeUI();
            EditorGUILayout.Space();
            if(GUILayout.Button("Custom Data Auto Setup"))
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).CustomDataAutoSetup();
            EditorGUILayout.Space();

            DrawLine(Color.grey, 1, 3);
            EditorGUILayout.LabelField("Particle System Options and Presets", headerBoldLabel, GUILayout.Height(20));
            
            EditorGUI.BeginChangeCheck();
            GeneralOptions();
            EmissionOptions();
            ShapeOptions();
            OverLifetimeOptions();
            ParticleHelperPresets();
            ParticleSystemPresets();

            EditorGUILayout.Space();

            if(GUILayout.Button("Fetch Particle System Current Values"))
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).FetchAllCurrentValues();

            bool applyEverythingOnChange = ((AllIn1ParticleHelperComponent)target).applyEverythingOnChange;

            if(!applyEverythingOnChange)
            {
                if(GUILayout.Button("Apply All Options Settings"))
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).ApplyCurrentSettings();
            }

            bool toggle = ((AllIn1ParticleHelperComponent)target).applyEverythingOnChange;
            EditorGUIUtility.labelWidth = 190;
            toggle = EditorGUILayout.Toggle("Auto Apply On Change Property", toggle);
            EditorGUIUtility.labelWidth = 145;
            if(toggle != ((AllIn1ParticleHelperComponent)target).applyEverythingOnChange)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).applyEverythingOnChange = toggle;

            EditorGUILayout.Space();
            DrawLine(Color.grey, 1, 3);
            EditorGUILayout.Space();
            if(GUILayout.Button("Remove Component"))
                for(int i = targets.Length - 1; i >= 0; i--)
                    DestroyImmediate(targets[i] as AllIn1ParticleHelperComponent);

            if(EditorGUI.EndChangeCheck() && applyEverythingOnChange)
            {
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).ApplyCurrentSettings();
            }
        }

        private void HierarchyHelpers()
        {
            bool toggle = ((AllIn1ParticleHelperComponent)target).hierarchyHelpers;
            toggle = EditorGUILayout.BeginToggleGroup("Hierarchy Helpers", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).hierarchyHelpers)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).hierarchyHelpers = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);
                if(GUILayout.Button("Add Child Copy"))
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).CopyCurrentGameObject(false);
                if(GUILayout.Button("Add Sibling Copy"))
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).CopyCurrentGameObject(true);
                int nCopies = ((AllIn1ParticleHelperComponent)target).numberOfCopies;
                EditorGUI.BeginChangeCheck();
                nCopies = EditorGUILayout.IntField("New Copy Number", nCopies);
                if(EditorGUI.EndChangeCheck())
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).numberOfCopies = nCopies;
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
        }

        private void ColorChangeUI()
        {
            bool toggle = ((AllIn1ParticleHelperComponent)target).colorChangeOption;
            toggle = EditorGUILayout.BeginToggleGroup("Palette Color Change", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).colorChangeOption)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).colorChangeOption = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);
                targetColor = EditorGUILayout.ColorField("New Color", targetColor);
                if(mainModule.startColor.mode == ParticleSystemGradientMode.Color ||
                   mainModule.startColor.mode == ParticleSystemGradientMode.TwoColors)
                {
                    if(GUILayout.Button("Recolor Particle System"))
                        for(int i = 0; i < targets.Length; i++)
                            ((AllIn1ParticleHelperComponent)targets[i]).HueShiftParticleSystem(targetColor);
                }
                else if(mainModule.startColor.mode == ParticleSystemGradientMode.Gradient ||
                        mainModule.startColor.mode == ParticleSystemGradientMode.RandomColor ||
                        mainModule.startColor.mode == ParticleSystemGradientMode.TwoGradients)
                {
                    EditorGUILayout.IntField("Gradient Reference Index", gradientColorKeyIndex, GUILayout.ExpandWidth(true));
                    if(GUILayout.Button("Recolor Particle System"))
                        for(int i = 0; i < targets.Length; i++)
                            ((AllIn1ParticleHelperComponent)targets[i]).HueShiftParticleSystem(targetColor, gradientColorKeyIndex);
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
        }

        private void GeneralOptions()
        {
            bool toggle = ((AllIn1ParticleHelperComponent)target).generalOptions;
            toggle = EditorGUILayout.BeginToggleGroup(helperNum + ".General Options", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).generalOptions)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).generalOptions = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);

                toggle = ((AllIn1ParticleHelperComponent)target).matchDurationToLifetime;
                EditorGUIUtility.labelWidth = 170;
                toggle = EditorGUILayout.Toggle("Match Duration To Lifetime?", toggle);
                EditorGUIUtility.labelWidth = 145;
                if(toggle != ((AllIn1ParticleHelperComponent)target).matchDurationToLifetime)
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).matchDurationToLifetime = toggle;

                toggle = ((AllIn1ParticleHelperComponent)target).randomRotation;
                toggle = EditorGUILayout.Toggle("Random Rotation?", toggle);
                if(toggle != ((AllIn1ParticleHelperComponent)target).randomRotation)
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).randomRotation = toggle;

                float minLifetime = ((AllIn1ParticleHelperComponent)target).minLifetime;
                float maxLifetime = ((AllIn1ParticleHelperComponent)target).maxLifetime;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider("Particle Lifetime", ref minLifetime, ref maxLifetime, 0, 30);
                minLifetime = EditorGUILayout.FloatField("Min Lifetime", minLifetime);
                maxLifetime = EditorGUILayout.FloatField("Max Lifetime", maxLifetime);
                if(EditorGUI.EndChangeCheck())
                {
                    minLifetime = Mathf.Clamp(minLifetime, 0, 1000);
                    maxLifetime = Mathf.Clamp(maxLifetime, 0, 1000);
                    if(minLifetime > maxLifetime) maxLifetime = minLifetime;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).minLifetime = minLifetime;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).maxLifetime = maxLifetime;
                }

                float minSpeed = ((AllIn1ParticleHelperComponent)target).minSpeed;
                float maxSpeed = ((AllIn1ParticleHelperComponent)target).maxSpeed;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider("Particle Speed", ref minSpeed, ref maxSpeed, 0, 30);
                minSpeed = EditorGUILayout.FloatField("Min Speed", minSpeed);
                maxSpeed = EditorGUILayout.FloatField("Max Speed", maxSpeed);
                if(EditorGUI.EndChangeCheck())
                {
                    minSpeed = Mathf.Clamp(minSpeed, 0, 1000);
                    maxSpeed = Mathf.Clamp(maxSpeed, 0, 1000);
                    if(minSpeed > maxSpeed) maxSpeed = minSpeed;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).minSpeed = minSpeed;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).maxSpeed = maxSpeed;
                }

                float minSize = ((AllIn1ParticleHelperComponent)target).minSize;
                float maxSize = ((AllIn1ParticleHelperComponent)target).maxSize;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider("Particle Size", ref minSize, ref maxSize, 0, 30);
                minSize = EditorGUILayout.FloatField("Min Size", minSize);
                maxSize = EditorGUILayout.FloatField("Max Size", maxSize);
                if(EditorGUI.EndChangeCheck())
                {
                    minSize = Mathf.Clamp(minSize, 0, 1000);
                    maxSize = Mathf.Clamp(maxSize, 0, 1000);
                    if(minSize > maxSize) maxSize = minSize;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).minSize = minSize;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).maxSize = maxSize;
                }

                EditorGUILayout.PropertyField(startColorProperty, new GUIContent("Start Color"));
                serializedObject.ApplyModifiedProperties();

                if(GUILayout.Button("Fetch Particle System General Values"))
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).FetchGeneralValues();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
            helperNum++;
        }

        private void EmissionOptions()
        {
            bool toggle = ((AllIn1ParticleHelperComponent)target).emissionOptions;
            toggle = EditorGUILayout.BeginToggleGroup(helperNum + ".Emission Options", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).emissionOptions)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).emissionOptions = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);

                toggle = ((AllIn1ParticleHelperComponent)target).isBurst;
                toggle = EditorGUILayout.Toggle("Use burst?", toggle);
                if(toggle != ((AllIn1ParticleHelperComponent)target).isBurst)
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).isBurst = toggle;

                float minNumberOfParticles = ((AllIn1ParticleHelperComponent)target).minNumberOfParticles;
                float maxNumberOfParticles = ((AllIn1ParticleHelperComponent)target).maxNumberOfParticles;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.MinMaxSlider("Number of Particles", ref minNumberOfParticles, ref maxNumberOfParticles, 0, 100);
                minNumberOfParticles = EditorGUILayout.IntField("Min Particles", Mathf.RoundToInt(minNumberOfParticles));
                maxNumberOfParticles = EditorGUILayout.IntField("Max Particles", Mathf.RoundToInt(maxNumberOfParticles));
                if(EditorGUI.EndChangeCheck())
                {
                    minNumberOfParticles = Mathf.Clamp(minNumberOfParticles, 0, 1000);
                    maxNumberOfParticles = Mathf.Clamp(maxNumberOfParticles, 0, 1000);
                    if(minNumberOfParticles > maxNumberOfParticles) maxNumberOfParticles = minNumberOfParticles;
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).minNumberOfParticles = Mathf.RoundToInt(minNumberOfParticles);
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).maxNumberOfParticles = Mathf.RoundToInt(maxNumberOfParticles);
                }

                if(GUILayout.Button("Fetch Particle System Emission Values"))
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).FetchEmissionValues();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
            helperNum++;
        }

        private void ShapeOptions()
        {
            bool toggle = ((AllIn1ParticleHelperComponent)target).shapeOptions;
            toggle = EditorGUILayout.BeginToggleGroup(helperNum + ".Shape Options", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).shapeOptions)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).shapeOptions = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);

                AllIn1ParticleHelperComponent.EmissionShapes tempSettings = ((AllIn1ParticleHelperComponent)target).currEmissionShape;
                EditorGUI.BeginChangeCheck();
                tempSettings = (AllIn1ParticleHelperComponent.EmissionShapes)EditorGUILayout.EnumPopup("New Shape", tempSettings);
                if(EditorGUI.EndChangeCheck())
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).currEmissionShape = tempSettings;

                if(GUILayout.Button("Fetch Particle System Shape Values"))
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).FetchShapeValues();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
            helperNum++;
        }

        private void OverLifetimeOptions()
        {
            bool toggle = ((AllIn1ParticleHelperComponent)target).overLifetimeOptions;
            toggle = EditorGUILayout.BeginToggleGroup(helperNum + ".Over Lifetime Options", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).overLifetimeOptions)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).overLifetimeOptions = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);

                AllIn1ParticleHelperComponent.LifetimeSettings tempColorLifetime = ((AllIn1ParticleHelperComponent)target).colorLifetime;
                EditorGUI.BeginChangeCheck();
                tempColorLifetime = (AllIn1ParticleHelperComponent.LifetimeSettings)EditorGUILayout.EnumPopup("Alpha Over Lifetime", tempColorLifetime);
                if(EditorGUI.EndChangeCheck())
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).colorLifetime = tempColorLifetime;

                AllIn1ParticleHelperComponent.LifetimeSettings tempScaleLifetime = ((AllIn1ParticleHelperComponent)target).sizeLifetime;
                EditorGUI.BeginChangeCheck();
                tempScaleLifetime = (AllIn1ParticleHelperComponent.LifetimeSettings)EditorGUILayout.EnumPopup("Scale Over Lifetime", tempScaleLifetime);
                if(EditorGUI.EndChangeCheck())
                    for(int i = 0; i < targets.Length; i++)
                        ((AllIn1ParticleHelperComponent)targets[i]).sizeLifetime = tempScaleLifetime;

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
            helperNum++;
        }

        private int selectedIndex = 0;
        private AllIn1ParticleHelperSO[] particleHelpersPresets = null;

        private void ParticleHelperPresets()
        {
            if(particleHelpersPresets == null)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).particleHelperPresets = false;

            bool toggle = ((AllIn1ParticleHelperComponent)target).particleHelperPresets;
            toggle = EditorGUILayout.BeginToggleGroup(helperNum + ".Particle Helper Presets", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).particleHelperPresets)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).particleHelperPresets = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);

                GUILayout.Label("Save Particle Helper Presets", smallBoldLabel);
                if(GUILayout.Button("Save Particle Helper Preset"))
                {
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).SaveParticleHelperPreset();
                    particleHelpersPresets = null;
                }

                DrawLine(Color.grey, 1, 3);

                GUILayout.Label("Load Particle Helper Presets", smallBoldLabel);
                if(particleHelpersPresets == null) particleHelpersPresets = Resources.LoadAll<AllIn1ParticleHelperSO>("");
                bool particleHelpersPresetsAvailable = particleHelpersPresets.Length > 0;
                if(!particleHelpersPresetsAvailable) GUILayout.Label("No Particle Helper Presets found, \nthey should be placed in a Resource folder", smallRedBoldLabel);
                else
                {
                    string[] presetNames = new string[particleHelpersPresets.Length];
                    for(int i = 0; i < particleHelpersPresets.Length; i++) presetNames[i] = particleHelpersPresets[i].name;

                    selectedIndex = EditorGUILayout.Popup("Particle Helper Presets", selectedIndex, presetNames);
                    if(GUILayout.Button("Apply Particle Helper Preset"))
                        for(int i = 0; i < targets.Length; i++)
                            ((AllIn1ParticleHelperComponent)targets[i]).ApplyParticleHelperPreset(particleHelpersPresets[selectedIndex], true);
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
            helperNum++;
        }

        private Preset[] particleSystemPresets = null;

        private void ParticleSystemPresets()
        {
            if(particleSystemPresets == null)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).particleSystemPresets = false;

            bool toggle = ((AllIn1ParticleHelperComponent)target).particleSystemPresets;
            toggle = EditorGUILayout.BeginToggleGroup(helperNum + ".Particle System Presets", toggle);
            if(toggle != ((AllIn1ParticleHelperComponent)target).particleSystemPresets)
                for(int i = 0; i < targets.Length; i++)
                    ((AllIn1ParticleHelperComponent)targets[i]).particleSystemPresets = toggle;
            if(toggle)
            {
                EditorGUILayout.BeginVertical(propertiesStyle);

                GUILayout.Label("Save Particle System Presets", smallBoldLabel);
                if(GUILayout.Button("Save Particle System Preset"))
                {
                    for(int i = 0; i < targets.Length; i++) ((AllIn1ParticleHelperComponent)targets[i]).SaveParticleSystemPreset();
                    particleSystemPresets = null;
                }

                DrawLine(Color.grey, 1, 3);

                GUILayout.Label("Load Particle System Presets", smallBoldLabel);
                if(particleSystemPresets == null) particleSystemPresets = Resources.LoadAll<Preset>("");
                bool particleSystemPresetsAvailable = particleSystemPresets.Length > 0;
                if(!particleSystemPresetsAvailable) GUILayout.Label("No Particle System Presets found, \nthey should be placed in a Resource folder", smallRedBoldLabel);
                else
                {
                    string[] presetNames = new string[particleSystemPresets.Length];
                    for(int i = 0; i < particleSystemPresets.Length; i++) presetNames[i] = particleSystemPresets[i].name;

                    selectedIndex = EditorGUILayout.Popup("Particle System Presets", selectedIndex, presetNames);

                    if(GUILayout.Button("Apply Particle System Preset"))
                        for(int i = 0; i < targets.Length; i++)
                            ((AllIn1ParticleHelperComponent)targets[i]).ApplyParticleSystemPreset(particleSystemPresets[selectedIndex]);
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndToggleGroup();
            helperNum++;
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