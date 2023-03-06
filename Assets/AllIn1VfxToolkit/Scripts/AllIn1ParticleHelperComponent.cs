using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Presets;
using UnityEditor.SceneManagement;

#endif

namespace AllIn1VfxToolkit
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("AllIn1VfxToolkit/AddAllIn1VfxParticleHelper")]
    public class AllIn1ParticleHelperComponent : MonoBehaviour
    {
        public bool hierarchyHelpers = false;
        public bool generalOptions = false;
        public bool shapeOptions = false;
        public bool emissionOptions = false;
        public bool overLifetimeOptions = false;
        public bool colorChangeOption = false;
        public bool particleHelperPresets = false;
        public bool particleSystemPresets = false;
        public int numberOfCopies = 1;
        public bool applyEverythingOnChange = true;

        //General Options
        public bool matchDurationToLifetime = false;
        public bool randomRotation = false;
        public float minLifetime = 5f, maxLifetime = 5f;
        public float minSpeed = 5f, maxSpeed = 5f;
        public float minSize = 1f, maxSize = 1f;
        public ParticleSystem.MinMaxGradient startColor;

        //Emission Options
        public bool isBurst = false;
        public int minNumberOfParticles = 10, maxNumberOfParticles = 10;

        //Shape Options
        public enum EmissionShapes
        {
            Cone,
            Sphere,
            Circle,
            None
        }

        public EmissionShapes currEmissionShape = EmissionShapes.Cone;

        //Lifetime Options
        public enum LifetimeSettings
        {
            Ascendant,
            Descendent,
            None
        }

        public LifetimeSettings colorLifetime = LifetimeSettings.None;
        public LifetimeSettings sizeLifetime = LifetimeSettings.None;

#if UNITY_EDITOR
        private float hueDif, saturationDif, targetHue, targetSaturation;
        private ParticleSystem ps;
        private ParticleSystem.MainModule mainModule;

        private void Start()
        {
            FetchAllCurrentValues();
        }

        public void HueShiftParticleSystem(Color tColor, int gradientColorKeyIndex = 0)
        {
            CacheParticleSystem();
            if(ps == null)
            {
                Debug.LogError("This GameObject: " + gameObject.name + " doesn't have a Particle System and it should, please double check what you are doing");
                return;
            }

            mainModule = ps.main;
            Color.RGBToHSV(tColor, out targetHue, out targetSaturation, out float _);

            switch(mainModule.startColor.mode)
            {
                case ParticleSystemGradientMode.Color:
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(ShiftColorAndSaveDif(mainModule.startColor.color));
                    break;

                case ParticleSystemGradientMode.Gradient:
                case ParticleSystemGradientMode.RandomColor:
                    Gradient shiftedGradient = GradientShiftAndSaveDif(mainModule.startColor.gradient, true, gradientColorKeyIndex);
                    mainModule.startColor = shiftedGradient;
                    break;

                case ParticleSystemGradientMode.TwoColors:
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(ShiftColorAndSaveDif(mainModule.startColor.colorMax),
                        ShiftColorFromPreCalculatedDif(mainModule.startColor.colorMin));
                    break;

                case ParticleSystemGradientMode.TwoGradients:
                    Gradient shiftedGradientMax = GradientShiftAndSaveDif(mainModule.startColor.gradientMax, true, 0);
                    Gradient shiftedGradientMin = GradientShiftFromPreCalculatedDif(mainModule.startColor.gradientMin);
                    mainModule.startColor = new ParticleSystem.MinMaxGradient(shiftedGradientMax, shiftedGradientMin);
                    break;
            }

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = ps.colorOverLifetime;
            if(colorOverLifetimeModule.enabled)
            {
                switch(colorOverLifetimeModule.color.mode)
                {
                    case ParticleSystemGradientMode.Color:
                        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(ShiftColorAndSaveDif(colorOverLifetimeModule.color.color));
                        break;

                    case ParticleSystemGradientMode.Gradient:
                    case ParticleSystemGradientMode.RandomColor:
                        Gradient shiftedGradient = GradientShiftAndSaveDif(colorOverLifetimeModule.color.gradient, true, gradientColorKeyIndex);
                        colorOverLifetimeModule.color = shiftedGradient;
                        break;

                    case ParticleSystemGradientMode.TwoColors:
                        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(ShiftColorAndSaveDif(colorOverLifetimeModule.color.colorMax),
                            ShiftColorFromPreCalculatedDif(colorOverLifetimeModule.color.colorMin));
                        break;

                    case ParticleSystemGradientMode.TwoGradients:
                        Gradient shiftedGradientMax = GradientShiftAndSaveDif(colorOverLifetimeModule.color.gradientMax, true, 0);
                        Gradient shiftedGradientMin = GradientShiftFromPreCalculatedDif(colorOverLifetimeModule.color.gradientMin);
                        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(shiftedGradientMax, shiftedGradientMin);
                        break;
                }
            }
            
            ParticleSystem.ColorBySpeedModule colorBySpeedModule = ps.colorBySpeed;
            if(colorBySpeedModule.enabled)
            {
                switch(colorBySpeedModule.color.mode)
                {
                    case ParticleSystemGradientMode.Color:
                        colorBySpeedModule.color = new ParticleSystem.MinMaxGradient(ShiftColorAndSaveDif(colorBySpeedModule.color.color));
                        break;

                    case ParticleSystemGradientMode.Gradient:
                    case ParticleSystemGradientMode.RandomColor:
                        Gradient shiftedGradient = GradientShiftAndSaveDif(colorBySpeedModule.color.gradient, true, gradientColorKeyIndex);
                        colorBySpeedModule.color = shiftedGradient;
                        break;

                    case ParticleSystemGradientMode.TwoColors:
                        colorBySpeedModule.color = new ParticleSystem.MinMaxGradient(ShiftColorAndSaveDif(colorBySpeedModule.color.colorMax),
                            ShiftColorFromPreCalculatedDif(colorBySpeedModule.color.colorMin));
                        break;

                    case ParticleSystemGradientMode.TwoGradients:
                        Gradient shiftedGradientMax = GradientShiftAndSaveDif(colorBySpeedModule.color.gradientMax, true, 0);
                        Gradient shiftedGradientMin = GradientShiftFromPreCalculatedDif(colorBySpeedModule.color.gradientMin);
                        colorBySpeedModule.color = new ParticleSystem.MinMaxGradient(shiftedGradientMax, shiftedGradientMin);
                        break;
                }
            }

            ResetParticleSystem();
            SetSceneDirty();
        }

        private void ResetParticleSystem()
        {
            CacheParticleSystem();
            if(ps == null) return;
            ps.Clear();
            ps.Play();
        }

        private void CacheParticleSystem()
        {
            if(ps != null) return;
            ps = GetComponent<ParticleSystem>();
        }

        private Gradient GradientShiftAndSaveDif(Gradient inGradient, bool saveColorOfFirstKey = false, int indexOfReferenceColor = 0)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[inGradient.colorKeys.Length];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[inGradient.alphaKeys.Length];
            for(int i = 0; i < colorKeys.Length; i++)
            {
                if(i == indexOfReferenceColor && saveColorOfFirstKey) colorKeys[i].color = ShiftColorAndSaveDif(inGradient.colorKeys[i].color);
                else colorKeys[i].color = ShiftColorFromPreCalculatedDif(inGradient.colorKeys[i].color);

                colorKeys[i].time = inGradient.colorKeys[i].time;
            }

            for(int i = 0; i < alphaKeys.Length; i++)
            {
                alphaKeys[i].alpha = inGradient.alphaKeys[i].alpha;
                alphaKeys[i].time = inGradient.alphaKeys[i].time;
            }

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
        
        private Gradient GradientShiftFromPreCalculatedDif(Gradient inGradient)
        {
            GradientColorKey[] colorKeys = new GradientColorKey[inGradient.colorKeys.Length];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[inGradient.alphaKeys.Length];
            for(int i = 0; i < colorKeys.Length; i++)
            {
                colorKeys[i].color = ShiftColorFromPreCalculatedDif(inGradient.colorKeys[i].color);
                colorKeys[i].time = inGradient.colorKeys[i].time;
            }

            for(int i = 0; i < alphaKeys.Length; i++)
            {
                alphaKeys[i].alpha = inGradient.alphaKeys[i].alpha;
                alphaKeys[i].time = inGradient.alphaKeys[i].time;
            }

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }

        private Color ShiftColorAndSaveDif(Color colorToShift)
        {
            Color.RGBToHSV(colorToShift, out float colorHue, out float colorSaturation, out float colorValue);
            hueDif = targetHue - colorHue;
            saturationDif = targetSaturation - colorSaturation;
            return Color.HSVToRGB(Mathf.Clamp01(colorHue + hueDif), Mathf.Clamp01(colorSaturation + saturationDif), colorValue);
        }

        private Color ShiftColorFromPreCalculatedDif(Color colorToShift)
        {
            Color.RGBToHSV(colorToShift, out float colorHue, out float colorSaturation, out float colorValue);
            return Color.HSVToRGB(Mathf.Clamp01(colorHue + hueDif), Mathf.Clamp01(colorSaturation + saturationDif), colorValue);
        }

        public void CopyCurrentGameObject(bool isSibling)
        {
            GameObject g = Instantiate(gameObject, transform.position, transform.rotation);
            g.GetComponent<AllIn1ParticleHelperComponent>().numberOfCopies = 1;
            string newName = gameObject.name;
            newName = newName.Split(char.Parse("_"))[0];
            newName += "_" + numberOfCopies;
            numberOfCopies++;
            g.name = newName;
            List<Transform> childrenToDelete = new List<Transform>();
            foreach(Transform child in g.transform) childrenToDelete.Add(child);
            foreach(Transform childDelete in childrenToDelete) DestroyImmediate(childDelete.gameObject);
            if(isSibling) g.transform.parent = transform.parent;
            else g.transform.parent = transform;
        }

        public void ApplyCurrentSettings(bool applyAllModules = false)
        {
            CacheParticleSystem();
            if(ps == null)
            {
                Debug.LogError("This GameObject: " + gameObject.name + " doesn't have a Particle System and it should, please double check what you are doing");
                return;
            }

            ps.Stop();
            ps.Clear();

            if(generalOptions || applyAllModules)
            {
                ParticleSystem.MainModule mainData = ps.main;
                if(matchDurationToLifetime)
                {
                    if(mainData.startLifetime.mode == ParticleSystemCurveMode.Curve) mainData.duration = mainData.startLifetime.curveMax.Evaluate(1f);
                    else if(mainData.startLifetime.mode == ParticleSystemCurveMode.TwoCurves) mainData.duration = Mathf.Max(mainData.startLifetime.curveMin.Evaluate(1f), mainData.startLifetime.curveMax.Evaluate(1f));
                    else if(mainData.startLifetime.mode == ParticleSystemCurveMode.TwoConstants) mainData.duration = Mathf.Max(mainData.startLifetime.constantMin, mainData.startLifetime.constantMax);
                    else mainData.duration = mainData.startLifetime.constantMax;
                }

                if(randomRotation) mainData.startRotation = new ParticleSystem.MinMaxCurve(0f, 6.28318f);
                else mainData.startRotation = new ParticleSystem.MinMaxCurve(0f);

                bool sameMinAndMax = Math.Abs(minLifetime - maxLifetime) < 0.01f;
                if(sameMinAndMax) mainData.startLifetime = new ParticleSystem.MinMaxCurve(maxLifetime);
                else mainData.startLifetime = new ParticleSystem.MinMaxCurve(minLifetime, maxLifetime);

                sameMinAndMax = Math.Abs(minSpeed - maxSpeed) < 0.01f;
                if(sameMinAndMax) mainData.startSpeed = new ParticleSystem.MinMaxCurve(maxSpeed);
                else mainData.startSpeed = new ParticleSystem.MinMaxCurve(minSpeed, maxSpeed);

                sameMinAndMax = Math.Abs(minSize - maxSize) < 0.01f;
                if(sameMinAndMax) mainData.startSize = new ParticleSystem.MinMaxCurve(maxSize);
                else mainData.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);

                mainData.startColor = startColor;
            }

            if(emissionOptions || applyAllModules)
            {
                ParticleSystem.EmissionModule emissionData = ps.emission;
                emissionData.enabled = true;
                bool sameMinAndMax = minNumberOfParticles == maxNumberOfParticles;
                if(isBurst)
                {
                    emissionData.rateOverTime = 0;
                    if(sameMinAndMax) emissionData.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(0f, maxNumberOfParticles)});
                    else emissionData.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(0f, new ParticleSystem.MinMaxCurve(minNumberOfParticles, maxNumberOfParticles))});
                }
                else
                {
                    if(sameMinAndMax) emissionData.rateOverTime = maxNumberOfParticles;
                    else emissionData.rateOverTime = new ParticleSystem.MinMaxCurve(minNumberOfParticles, maxNumberOfParticles);
                    emissionData.SetBursts(new ParticleSystem.Burst[0]);
                }
            }

            if(shapeOptions || applyAllModules)
            {
                ParticleSystem.ShapeModule shapeData = ps.shape;
                shapeData.enabled = true;
                switch(currEmissionShape)
                {
                    case EmissionShapes.Circle:
                        shapeData.shapeType = ParticleSystemShapeType.Circle;
                        break;
                    case EmissionShapes.Cone:
                        shapeData.shapeType = ParticleSystemShapeType.Cone;
                        break;
                    case EmissionShapes.Sphere:
                        shapeData.shapeType = ParticleSystemShapeType.Sphere;
                        break;
                    case EmissionShapes.None:
                        shapeData.enabled = false;
                        break;
                }
            }

            if(overLifetimeOptions || applyAllModules)
            {
                ParticleSystem.ColorOverLifetimeModule colorLifeData = ps.colorOverLifetime;
                colorLifeData.enabled = true;
                Gradient newGradient = new Gradient();
                switch(colorLifetime)
                {
                    case LifetimeSettings.Ascendant:
                        newGradient.SetKeys(
                            new GradientColorKey[] {new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f)},
                            new GradientAlphaKey[] {new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0.65f, 0.5f), new GradientAlphaKey(1f, 1.0f)}
                        );
                        colorLifeData.color = newGradient;
                        break;
                    case LifetimeSettings.Descendent:
                        newGradient.SetKeys(
                            new GradientColorKey[] {new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f)},
                            new GradientAlphaKey[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.25f), new GradientAlphaKey(0f, 1.0f)}
                        );
                        colorLifeData.color = newGradient;
                        break;
                    case LifetimeSettings.None:
                        colorLifeData.enabled = false;
                        break;
                }

                ParticleSystem.SizeOverLifetimeModule sizeLifeData = ps.sizeOverLifetime;
                sizeLifeData.enabled = true;
                AnimationCurve newAnimationCurve = new AnimationCurve();
                switch(sizeLifetime)
                {
                    case LifetimeSettings.Ascendant:
                        newAnimationCurve.AddKey(0.0f, 0.0f);
                        newAnimationCurve.AddKey(0.5f, 0.5f);
                        newAnimationCurve.AddKey(1.0f, 1.0f);
                        sizeLifeData.size = new ParticleSystem.MinMaxCurve(1f, newAnimationCurve);
                        break;
                    case LifetimeSettings.Descendent:
                        newAnimationCurve.AddKey(0.0f, 1.0f);
                        newAnimationCurve.AddKey(0.5f, 0.5f);
                        newAnimationCurve.AddKey(1.0f, 0.0f);
                        sizeLifeData.size = new ParticleSystem.MinMaxCurve(1f, newAnimationCurve);
                        break;
                    case LifetimeSettings.None:
                        sizeLifeData.enabled = false;
                        break;
                }
            }

            ResetParticleSystem();
            SetSceneDirty();
        }

        public void FetchAllCurrentValues()
        {
            FetchGeneralValues();
            FetchEmissionValues();
            FetchShapeValues();
        }
        
        public void FetchGeneralValues()
        {
            CacheParticleSystem();
            
            ParticleSystem.MainModule mainData = ps.main;

            if(mainData.startRotation.mode == ParticleSystemCurveMode.Constant) randomRotation = false;
            else randomRotation = true;
            
            if(mainData.startLifetime.mode == ParticleSystemCurveMode.Constant) minLifetime = mainData.startLifetime.constantMax;
            else  minLifetime = mainData.startLifetime.constantMin;
            maxLifetime = mainData.startLifetime.constantMax;
            
            if(mainData.startSpeed.mode == ParticleSystemCurveMode.Constant) minSpeed = mainData.startSpeed.constantMax;
            else  minSpeed = mainData.startSpeed.constantMin;
            maxSpeed = mainData.startSpeed.constantMax;
            
            if(mainData.startSize.mode == ParticleSystemCurveMode.Constant) minSize = mainData.startSize.constantMax;
            else  minSize = mainData.startSize.constantMin;
            maxSize = mainData.startSize.constantMax;

            startColor = mainData.startColor;
        }
        
        public void FetchEmissionValues()
        {
            CacheParticleSystem();
            
            ParticleSystem.EmissionModule emissionData = ps.emission;

            isBurst = emissionData.burstCount > 0;

            if(isBurst)
            {
                ParticleSystem.Burst burst = emissionData.GetBurst(0);
                minNumberOfParticles = burst.minCount;
                maxNumberOfParticles = burst.maxCount;
                if(minNumberOfParticles == 0) minNumberOfParticles = maxNumberOfParticles;
            }
            else
            {
                if(emissionData.rateOverTime.mode == ParticleSystemCurveMode.Constant) minNumberOfParticles = (int) emissionData.rateOverTime.constantMax;
                else minNumberOfParticles = (int) emissionData.rateOverTime.constantMin;
                maxNumberOfParticles = (int) emissionData.rateOverTime.constantMax;
            }
        }

        public void FetchShapeValues()
        {
            CacheParticleSystem();
            
            ParticleSystem.ShapeModule shapeData = ps.shape;
            switch(shapeData.shapeType)
            {
                case ParticleSystemShapeType.Sphere:
                    currEmissionShape = EmissionShapes.Sphere;
                    break;
                case ParticleSystemShapeType.Cone:
                    currEmissionShape = EmissionShapes.Cone;
                    break;
                case ParticleSystemShapeType.ConeVolume:
                    currEmissionShape = EmissionShapes.Cone;
                    break;
                case ParticleSystemShapeType.Circle:
                    currEmissionShape = EmissionShapes.Circle;
                    break;
                default:
                    currEmissionShape = EmissionShapes.None;
                    break;
            }
        }

        public void ApplyParticleHelperPreset(AllIn1ParticleHelperSO particleHelpersPreset, bool applyAllModules = false)
        {
            numberOfCopies = 1;

            matchDurationToLifetime = particleHelpersPreset.matchDurationToLifetime;
            randomRotation = particleHelpersPreset.randomRotation;
            minLifetime = particleHelpersPreset.minLifetime;
            maxLifetime = particleHelpersPreset.maxLifetime;
            minSpeed = particleHelpersPreset.minSpeed;
            maxSpeed = particleHelpersPreset.maxSpeed;
            minSize = particleHelpersPreset.minSize;
            maxSize = particleHelpersPreset.maxSize;
            startColor = particleHelpersPreset.startColor;

            isBurst = particleHelpersPreset.isBurst;
            minNumberOfParticles = particleHelpersPreset.minNumberOfParticles;
            maxNumberOfParticles = particleHelpersPreset.maxNumberOfParticles;

            currEmissionShape = particleHelpersPreset.currEmissionShape;

            colorLifetime = particleHelpersPreset.colorLifetime;
            sizeLifetime = particleHelpersPreset.sizeLifetime;

            ApplyCurrentSettings(applyAllModules);
        }

        public void SaveParticleHelperPreset()
        {
            AllIn1ParticleHelperSO psHelperPresetAsset = ScriptableObject.CreateInstance<AllIn1ParticleHelperSO>();

            psHelperPresetAsset.matchDurationToLifetime = matchDurationToLifetime;
            psHelperPresetAsset.randomRotation = randomRotation;
            psHelperPresetAsset.minLifetime = minLifetime;
            psHelperPresetAsset.maxLifetime = maxLifetime;
            psHelperPresetAsset.minSpeed = minSpeed;
            psHelperPresetAsset.maxSpeed = maxSpeed;
            psHelperPresetAsset.minSize = minSize;
            psHelperPresetAsset.maxSize = maxSize;
            psHelperPresetAsset.startColor = startColor;

            psHelperPresetAsset.isBurst = isBurst;
            psHelperPresetAsset.minNumberOfParticles = minNumberOfParticles;
            psHelperPresetAsset.maxNumberOfParticles = maxNumberOfParticles;

            psHelperPresetAsset.currEmissionShape = currEmissionShape;

            psHelperPresetAsset.colorLifetime = colorLifetime;
            psHelperPresetAsset.sizeLifetime = sizeLifetime;

            string path = "Assets/AllIn1VfxToolkit/ParticlePresets/Resources";
            if(PlayerPrefs.HasKey("All1VfxParticlePresets")) path = PlayerPrefs.GetString("All1VfxParticlePresets") + "/";
            else PlayerPrefs.SetString("All1VfxParticlePresets", "Assets/AllIn1VfxToolkit/ParticlePresets/Resources");
            if(!System.IO.Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("The desired Particle Presets Save Path doesn't exist",
                    "Go to Window -> AllIn1VfxWindow and set a valid folder", "Ok");
                return;
            }

            string fullPath = path + gameObject.name + "HelperPreset" + ".asset";
            if(System.IO.File.Exists(fullPath)) fullPath = GetNewValidPath(path + gameObject.name + "HelperPreset", ".asset");

            string fileName = fullPath.Replace(path, "");
            fileName = fileName.Replace(".asset", "");
            fullPath = EditorUtility.SaveFilePanel("Save Particle Helper Preset", path, fileName, "asset");
            if(fullPath.Length == 0) return;
            fullPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            AssetDatabase.CreateAsset(psHelperPresetAsset, fullPath);
            AssetDatabase.SaveAssets();

            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(fullPath, typeof(AllIn1ParticleHelperSO)));
        }

        
        public void ApplyParticleSystemPreset(Preset particleSystemPreset)
        {
            CacheParticleSystem();
            if(ps == null)
            {
                Debug.LogError("This GameObject: " + gameObject.name + " doesn't have a Particle System and it should, please double check what you are doing");
                return;
            }
            
            particleSystemPreset.ApplyTo(ps);
            ResetParticleSystem();
            SetSceneDirty();
        }
        
        public void SaveParticleSystemPreset()
        {
            CacheParticleSystem();
            if(ps == null)
            {
                Debug.LogError("This GameObject: " + gameObject.name + " doesn't have a Particle System and it should, please double check what you are doing");
                return;
            }

            Preset psPreset = new Preset(ps);
            
            string path = "Assets/AllIn1VfxToolkit/ParticlePresets/Resources";
            if(PlayerPrefs.HasKey("All1VfxParticlePresets")) path = PlayerPrefs.GetString("All1VfxParticlePresets") + "/";
            else PlayerPrefs.SetString("All1VfxParticlePresets", "Assets/AllIn1VfxToolkit/ParticlePresets/Resources");
            if(!System.IO.Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("The desired Particle Presets Save Path doesn't exist",
                    "Go to Window -> AllIn1VfxWindow and set a valid folder", "Ok");
                return;
            }

            string fullPath = path + gameObject.name + "PsPreset" + ".preset";
            if(System.IO.File.Exists(fullPath)) fullPath = GetNewValidPath(path + gameObject.name + "PsPreset", ".preset");

            string fileName = fullPath.Replace(path, "");
            fileName = fileName.Replace(".preset", "");
            fullPath = EditorUtility.SaveFilePanel("Save Particle Helper Preset", path, fileName, "preset");
            if(fullPath.Length == 0) return;
            fullPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            AssetDatabase.CreateAsset(psPreset, fullPath);
            AssetDatabase.SaveAssets();

            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(fullPath, typeof(Preset)));
        }

        private string GetNewValidPath(string path, string extension, int i = 1)
        {
            int number = i;
            string newPath = path + "_" + number.ToString();
            string fullPath = newPath + extension;
            if(System.IO.File.Exists(fullPath))
            {
                number++;
                fullPath = GetNewValidPath(path, extension, number);
            }
            
            return fullPath;
        }

        [ContextMenu("Delete all Particle Helpers in Hierarchy")]
        private void DeleteAllParticleHelpersInHierarchy()
        {
            StartCoroutine(DeleteAllChildHelperComponentsCR());
            StartCoroutine(DelayedDestroyCR());
        }

        [ContextMenu("Delete child Particle Helpers")]
        private void DeleteChildParticleHelpers()
        {
            StartCoroutine(DeleteAllChildHelperComponentsCR());
        }

        private IEnumerator DelayedDestroyCR()
        {
            yield return null;
            DestroyImmediate(this);
        }

        private IEnumerator DeleteAllChildHelperComponentsCR()
        {
            yield return null;
            AllIn1ParticleHelperComponent[] helpers = gameObject.GetComponentsInChildren<AllIn1ParticleHelperComponent>();
            for(int i = 0; i < helpers.Length; i++)
                if(!helpers[i].gameObject.Equals(gameObject))
                    DestroyImmediate(helpers[i]);
        }

        public void CustomDataAutoSetup()
        {
            CacheParticleSystem();
            if(ps == null)
            {
                Debug.LogError("This GameObject: " + gameObject.name + " doesn't have a Particle System and it should, please double check what you are doing");
                return;
            }

            ParticleSystem.CustomDataModule customData = ps.customData;
            customData.enabled = true;

            customData.SetMode(ParticleSystemCustomData.Custom1, ParticleSystemCustomDataMode.Vector);
            customData.SetVectorComponentCount(ParticleSystemCustomData.Custom1, 1);
            customData.SetVector(ParticleSystemCustomData.Custom1, 0, new ParticleSystem.MinMaxCurve(0, 100));

            Renderer targetRenderer = ps.GetComponent<Renderer>();
            Material targetMaterial = targetRenderer.sharedMaterial;
            if(targetMaterial == null)
            {
                Debug.LogError("The Particle System in the object: " + gameObject.name + " has no valid target material");
                return;
            } 
            
            bool alphaEnable = targetMaterial.IsKeywordEnabled("ALPHAFADEINPUTSTREAM_ON") && (targetMaterial.IsKeywordEnabled("FADE_ON") || targetMaterial.IsKeywordEnabled("ALPHAFADE_ON"));
            if(alphaEnable)
            {
                customData.SetVectorComponentCount(ParticleSystemCustomData.Custom1, 2);
                AnimationCurve alphaFadeCurve = new AnimationCurve();
                alphaFadeCurve.AddKey(0.0f, 1.0f);
                alphaFadeCurve.AddKey(1.0f, 0.0f);
                customData.SetVector(ParticleSystemCustomData.Custom1, 1, new ParticleSystem.MinMaxCurve(1, alphaFadeCurve));
            }
            else customData.SetVectorComponentCount(ParticleSystemCustomData.Custom1, 1);

            bool texOffsetEnable = targetMaterial.IsKeywordEnabled("OFFSETSTREAM_ON");
            if(texOffsetEnable)
            {
                customData.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Vector);
                customData.SetVectorComponentCount(ParticleSystemCustomData.Custom2, 2);
                AnimationCurve textureOffsetCurve = new AnimationCurve();
                textureOffsetCurve.AddKey(0.0f, 0.0f);
                textureOffsetCurve.AddKey(1.0f, 1.0f);
                customData.SetVector(ParticleSystemCustomData.Custom2, 0, new ParticleSystem.MinMaxCurve(1, textureOffsetCurve));
                customData.SetVector(ParticleSystemCustomData.Custom2, 1, new ParticleSystem.MinMaxCurve(1, textureOffsetCurve));
            }

            bool shapetWeightsEnable = targetMaterial.IsKeywordEnabled("SHAPEWEIGHTS_ON");
            if(shapetWeightsEnable)
            {
                customData.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Vector);
                customData.SetVectorComponentCount(ParticleSystemCustomData.Custom2, 3);
                AnimationCurve weightOffsetCurve = new AnimationCurve();
                weightOffsetCurve.AddKey(0.0f, 0.0f);
                weightOffsetCurve.AddKey(1.0f, 1.0f);
                customData.SetVector(ParticleSystemCustomData.Custom2, 2, new ParticleSystem.MinMaxCurve(1, weightOffsetCurve));
            }
            
            if(!texOffsetEnable && !shapetWeightsEnable) customData.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Disabled);

            ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
            List<ParticleSystemVertexStream> rendererStreams = new List<ParticleSystemVertexStream>();
            rendererStreams.Add(ParticleSystemVertexStream.Position);
            rendererStreams.Add(ParticleSystemVertexStream.Normal);
            rendererStreams.Add(ParticleSystemVertexStream.Color);
            rendererStreams.Add(ParticleSystemVertexStream.UV);
            if(alphaEnable || texOffsetEnable || shapetWeightsEnable) rendererStreams.Add(ParticleSystemVertexStream.Custom1XY);
            else rendererStreams.Add(ParticleSystemVertexStream.Custom1X);
            
            //Unity throws an error warning if we don't add this also when only needing Fade for some reason, so we also send Custom2 data when only needing Fade to avoid the error warning
            if(shapetWeightsEnable) rendererStreams.Add(ParticleSystemVertexStream.Custom2XYZ);
            else if(texOffsetEnable) rendererStreams.Add(ParticleSystemVertexStream.Custom2XY);
            psRenderer.SetActiveVertexStreams(rendererStreams);
        }
#endif
        private void SetSceneDirty()
        {
#if UNITY_EDITOR
            if(!Application.isPlaying) EditorSceneManager.MarkAllScenesDirty();

            //If you get an error here please delete the 2 lines below
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if(prefabStage != null) EditorSceneManager.MarkSceneDirty(prefabStage.scene);
#endif
        }
    }
}