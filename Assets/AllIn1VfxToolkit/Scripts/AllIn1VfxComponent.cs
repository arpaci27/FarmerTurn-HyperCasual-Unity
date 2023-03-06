using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#endif

namespace AllIn1VfxToolkit
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("AllIn1VfxToolkit/AddAllIn1Vfx")]
    public class AllIn1VfxComponent : MonoBehaviour
    {
        private Material currMaterial, prevMaterial;
        private bool matAssigned = false, destroyed = false;

        private enum AfterSetAction
        {
            Clear,
            CopyMaterial,
            Reset
        };

#if UNITY_EDITOR
        private static float timeLastReload = -1f;

        private void Start()
        {
            if(timeLastReload < 0) timeLastReload = Time.time;
        }

        private void Update()
        {
            if(matAssigned || Application.isPlaying || !gameObject.activeSelf) return;
            Renderer sr = GetComponent<Renderer>();
            if(sr != null)
            {
                if(sr.sharedMaterial == null)
                {
                    CleanMaterial();
                    MakeNewMaterial();
                }

                if(!sr.sharedMaterial.shader.name.Contains("Vfx")) MakeNewMaterial();
                else matAssigned = true;
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                if(img != null)
                {
                    if(!img.material.shader.name.Contains("Vfx")) MakeNewMaterial();
                    else matAssigned = true;
                }
            }
        }
#endif

        private void MakeNewMaterial(string shaderName = "AllIn1Vfx")
        {
            SetMaterial(AfterSetAction.Clear, shaderName);
        }

        public void MakeCopy()
        {
            if(currMaterial == null)
            {
                if(FetchCurrentMaterial()) return;
            }

            string shaderName = currMaterial.shader.name;
            if(shaderName.Contains("AllIn1Vfx/")) shaderName = shaderName.Replace("AllIn1Vfx/", "");
            SetMaterial(AfterSetAction.CopyMaterial, shaderName);
        }

        private bool FetchCurrentMaterial()
        {
            bool rendererExists = false;
            Renderer sr = GetComponent<Renderer>();
            if(sr != null)
            {
                rendererExists = true;
                currMaterial = sr.sharedMaterial;
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                if(img != null)
                {
                    rendererExists = true;
                    currMaterial = img.material;
                }
            }

            if(!rendererExists)
            {
                MissingRenderer();
                return true;
            }

            return false;
        }

        private void ResetAllProperties(string shaderName)
        {
            SetMaterial(AfterSetAction.Reset, shaderName);
        }

        private void SetMaterial(AfterSetAction action, string shaderName)
        {
            Shader allIn1VfxShader = Resources.Load(shaderName, typeof(Shader)) as Shader;

            if(!Application.isPlaying && Application.isEditor && allIn1VfxShader != null)
            {
                bool rendererExists = false;
                Renderer sr = GetComponent<Renderer>();
                if(sr != null)
                {
                    rendererExists = true;
                    prevMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
                    currMaterial = new Material(allIn1VfxShader);
                    GetComponent<Renderer>().sharedMaterial = currMaterial;
                    GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
                    matAssigned = true;
                    DoAfterSetAction(action);
                }
                else
                {
                    Graphic img = GetComponent<Graphic>();
                    if(img != null)
                    {
                        rendererExists = true;
                        prevMaterial = new Material(img.material);
                        currMaterial = new Material(allIn1VfxShader);
                        img.material = currMaterial;
                        img.material.hideFlags = HideFlags.None;
                        matAssigned = true;
                        DoAfterSetAction(action);
                    }
                }

                if(!rendererExists)
                {
                    MissingRenderer();
                    return;
                }
                else
                {
                    SetSceneDirty();
                }
            }
            else if(allIn1VfxShader == null)
            {
                Debug.LogError(
                    "Make sure the AllIn1Vfx shader variants are inside the Resource folder!   You looked for " +
                    shaderName);
            }
        }

        private void DoAfterSetAction(AfterSetAction action)
        {
            switch(action)
            {
                case AfterSetAction.Clear:
                    ClearAllKeywords();
                    break;
                case AfterSetAction.CopyMaterial:
                    currMaterial.CopyPropertiesFromMaterial(prevMaterial);
                    break;
            }
        }

        public void TryCreateNew()
        {
            bool rendererExists = false;
            Renderer sr = GetComponent<Renderer>();
            if(sr != null)
            {
                rendererExists = true;
                if(sr != null && sr.sharedMaterial != null && sr.sharedMaterial.shader.name.Contains("Vfx"))
                {
                    ResetAllProperties("AllIn1Vfx");
                    ClearAllKeywords();
                }
                else
                {
                    CleanMaterial();
                    MakeNewMaterial("AllIn1Vfx");
                }
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                if(img != null)
                {
                    rendererExists = true;
                    if(img.material.shader.name.Contains("Vfx"))
                    {
                        ResetAllProperties("AllIn1Vfx");
                        ClearAllKeywords();
                    }
                    else MakeNewMaterial("AllIn1Vfx");
                }
            }

            if(!rendererExists)
            {
                MissingRenderer();
            }

            SetSceneDirty();
        }

        public void ClearAllKeywords()
        {
            SetKeyword("FOG_ON");
            SetKeyword("SCREENDISTORTION_ON");
            SetKeyword("DISTORTUSECOL_ON");
            SetKeyword("DISTORTONLYBACK_ON");
            SetKeyword("SHAPE1SCREENUV_ON");
            SetKeyword("SHAPE2SCREENUV_ON");
            SetKeyword("SHAPE3SCREENUV_ON");

            SetKeyword("SHAPEDEBUG_ON");

            SetKeyword("SHAPE1CONTRAST_ON");
            SetKeyword("SHAPE1DISTORT_ON");
            SetKeyword("SHAPE1ROTATE_ON");
            SetKeyword("SHAPE1SHAPECOLOR_ON");

            SetKeyword("SHAPE2_ON");
            SetKeyword("SHAPE2CONTRAST_ON");
            SetKeyword("SHAPE2DISTORT_ON");
            SetKeyword("SHAPE2ROTATE_ON");
            SetKeyword("SHAPE2SHAPECOLOR_");

            SetKeyword("SHAPE3_ON");
            SetKeyword("SHAPE3CONTRAST_ON");
            SetKeyword("SHAPE3DISTORT_ON");
            SetKeyword("SHAPE3ROTATE_ON");
            SetKeyword("SHAPE3SHAPECOLOR_");

            SetKeyword("GLOW_ON");
            SetKeyword("GLOWTEX_ON");
            SetKeyword("SOFTPART_ON");
            SetKeyword("DEPTHGLOW_ON");
            SetKeyword("MASK_ON");
            SetKeyword("COLORRAMP_ON");
            SetKeyword("COLORRAMPGRAD_ON");
            SetKeyword("COLORGRADING_ON");
            SetKeyword("HSV_ON");
            SetKeyword("BLUR_ON");
            SetKeyword("BLURISHD_ON");
            SetKeyword("POSTERIZE_ON");
            SetKeyword("FADE_ON");
            SetKeyword("FADEBURN_ON");
            SetKeyword("PIXELATE_ON");
            SetKeyword("DISTORT_ON");
            SetKeyword("SHAKEUV_ON");
            SetKeyword("WAVEUV_ON");
            SetKeyword("ROUNDWAVEUV_ON");
            SetKeyword("TWISTUV_ON");
            SetKeyword("DOODLE_ON");
            SetKeyword("OFFSETSTREAM_ON");
            SetKeyword("TEXTURESCROLL_ON");
            SetKeyword("VERTOFFSET_ON");
            SetKeyword("RIM_ON");
            SetKeyword("BACKFACETINT_ON");
            SetKeyword("POLARUV_ON");
            SetKeyword("POLARUVDISTORT_ON");
            SetKeyword("SHAPE1MASK_ON");
            SetKeyword("TRAILWIDTH_ON");
            SetKeyword("LIGHTANDSHADOW_ON");
            SetKeyword("SHAPETEXOFFSET_ON");
            SetKeyword("SHAPEWEIGHTS_ON");

            SetKeyword("ALPHACUTOFF_ON");
            SetKeyword("ALPHASMOOTHSTEP_ON");
            SetKeyword("ALPHAFADE_ON");
            SetKeyword("ALPHAFADEUSESHAPE1_");
            SetKeyword("ALPHAFADEUSEREDCHAN");
            SetKeyword("ALPHAFADETRANSPAREN");
            SetKeyword("ALPHAFADEINPUTSTREA");
            SetKeyword("CAMDISTFADE_ON");
            SetSceneDirty();
        }

        private void SetKeyword(string keyword, bool state = false)
        {
            if(destroyed) return;
            if(currMaterial == null)
            {
                FindCurrMaterial();
                if(currMaterial == null)
                {
                    MissingRenderer();
                    return;
                }
            }

            if(!state) currMaterial.DisableKeyword(keyword);
            else currMaterial.EnableKeyword(keyword);
        }

        private void FindCurrMaterial()
        {
            Renderer sr = GetComponent<Renderer>();
            if(sr != null)
            {
                currMaterial = GetComponent<Renderer>().sharedMaterial;
                matAssigned = true;
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                if(img != null)
                {
                    currMaterial = img.material;
                    matAssigned = true;
                }
            }
        }

        public void CleanMaterial()
        {
            Renderer sr = GetComponent<Renderer>();
            if(sr != null)
            {
                sr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
                matAssigned = false;
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                if(img != null)
                {
                    img.material = new Material(Shader.Find("Sprites/Default"));
                    matAssigned = false;
                }
            }

            SetSceneDirty();
        }

        public void SaveMaterial()
        {
#if UNITY_EDITOR
            string path = AllIn1VfxWindow.materialsSavesPath;
            if(PlayerPrefs.HasKey("All1VfxMaterials")) path = PlayerPrefs.GetString("All1VfxMaterials");
            else PlayerPrefs.SetString("All1VfxMaterials", AllIn1VfxWindow.materialsSavesPath);
            path += "/";
            if(!System.IO.Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("The desired save folder doesn't exist",
                    "Go to Window -> AllIn1VfxWindow and set a valid folder", "Ok");
                return;
            }

            path += gameObject.name;
            string fullPath = path + ".mat";
            if(System.IO.File.Exists(fullPath))
            {
                SaveMaterialWithOtherName(path);
            }
            else DoSaving(fullPath);

            SetSceneDirty();
#endif
        }

        private void SaveMaterialWithOtherName(string path, int i = 1)
        {
            int number = i;
            string newPath = path + "_" + number.ToString();
            string fullPath = newPath + ".mat";
            if(System.IO.File.Exists(fullPath))
            {
                number++;
                SaveMaterialWithOtherName(path, number);
            }
            else
            {
                DoSaving(fullPath);
            }
        }

        private void DoSaving(string fileName)
        {
#if UNITY_EDITOR
            bool rendererExists = false;
            Renderer sr = GetComponent<Renderer>();
            Material matToSave = null;
            Material createdMat = null;
            if(sr != null)
            {
                rendererExists = true;
                matToSave = sr.sharedMaterial;
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                if(img != null)
                {
                    rendererExists = true;
                    matToSave = img.material;
                }
            }

            if(!rendererExists)
            {
                MissingRenderer();
                return;
            }
            else
            {
                createdMat = new Material(matToSave);
                currMaterial = createdMat;
                AssetDatabase.CreateAsset(createdMat, fileName);
                Debug.Log(fileName + " has been saved!");
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(fileName, typeof(Material)));
            }

            if(sr != null)
            {
                sr.material = createdMat;
            }
            else
            {
                Graphic img = GetComponent<Graphic>();
                img.material = createdMat;
            }
#endif
        }

        public void SetSceneDirty()
        {
#if UNITY_EDITOR
            if(!Application.isPlaying) EditorSceneManager.MarkAllScenesDirty();

            //If you get an error here please delete the 2 lines below
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if(prefabStage != null) EditorSceneManager.MarkSceneDirty(prefabStage.scene);
#endif
        }

        private void MissingRenderer()
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Missing Renderer", "This GameObject (" +
                                                            gameObject.name +
                                                            ") has no Renderer or UI Graphic component. This AllIn1Vfx component will be removed.",
                "Ok");
            destroyed = true;
            DestroyImmediate(this);
#endif
        }

#if UNITY_EDITOR
        public void ApplyMaterialToHierarchy()
        {
            Renderer sr = GetComponent<Renderer>();
            Graphic img = GetComponent<Graphic>();
            Material matToApply = null;
            if(sr != null) matToApply = sr.sharedMaterial;
            else if(img != null)
            {
                matToApply = img.material;
            }
            else
            {
                MissingRenderer();
                return;
            }

            List<Transform> children = new List<Transform>();
            GetAllChildren(transform, ref children);
            foreach(Transform t in children)
            {
                sr = t.gameObject.GetComponent<Renderer>();
                if(sr != null) sr.material = matToApply;
                else
                {
                    img = t.gameObject.GetComponent<Graphic>();
                    if(img != null) img.material = matToApply;
                }
            }
        }

        public void CheckIfValidTarget()
        {
            Renderer sr = GetComponent<Renderer>();
            Graphic img = GetComponent<Graphic>();
            if(sr == null && img == null) MissingRenderer();
        }

        private void GetAllChildren(Transform parent, ref List<Transform> transforms)
        {
            foreach(Transform child in parent)
            {
                transforms.Add(child);
                GetAllChildren(child, ref transforms);
            }
        }

        public void RenderToImage()
        {
            if(currMaterial == null)
            {
                FindCurrMaterial();
                if(currMaterial == null)
                {
                    MissingRenderer();
                    return;
                }
            }

            Texture tex = currMaterial.GetTexture("_MainTex");
            if(tex != null) RenderAndSaveTexture(currMaterial, tex);
            else
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                Graphic i = GetComponent<Graphic>();
                if(sr != null) tex = sr.sprite.texture;
                else if(i != null) tex = i.mainTexture;

                if(tex != null) RenderAndSaveTexture(currMaterial, tex);
                else
                    EditorUtility.DisplayDialog("No valid target texture found",
                        "All In 1 VFX component couldn't find a valid Main Texture in this GameObject (" +
                        gameObject.name +
                        "). This means that the material you are using has no Main Texture or that the texture couldn't be reached through the Renderer component you are using." +
                        " Please make sure to have a valid Main Texture in the Material", "Ok");
            }
        }

        private void RenderAndSaveTexture(Material targetMaterial, Texture targetTexture)
        {
            float scaleSlider = 1;
            if(PlayerPrefs.HasKey("All1VfxRenderImagesScale"))
                scaleSlider = PlayerPrefs.GetFloat("All1VfxRenderImagesScale");
            RenderTexture renderTarget = new RenderTexture((int)(targetTexture.width * scaleSlider),
                (int)(targetTexture.height * scaleSlider), 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(targetTexture, renderTarget, targetMaterial);
            Texture2D resultTex = new Texture2D(renderTarget.width, renderTarget.height, TextureFormat.ARGB32, false);
            resultTex.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);
            resultTex.Apply();

            string path = AllIn1VfxWindow.renderImagesSavesPath;
            if(PlayerPrefs.HasKey("All1VfxRenderImages")) path = PlayerPrefs.GetString("All1VfxRenderImages");
            else PlayerPrefs.SetString("All1VfxRenderImages", AllIn1VfxWindow.renderImagesSavesPath);
            path +=  "/";
            if(!System.IO.Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("The desired Material to Image Save Path doesn't exist",
                    "Go to Window -> AllIn1VfxWindow and set a valid folder", "Ok");
                return;
            }

            string fullPath = path + gameObject.name + ".png";
            if(System.IO.File.Exists(fullPath)) fullPath = GetNewValidPath(path + gameObject.name);
            string pingPath = fullPath;

            string fileName = fullPath.Replace(path, "");
            fileName = fileName.Replace(".png", "");
            fullPath = EditorUtility.SaveFilePanel("Save Render Image", path, fileName, "png");
            if(fullPath.Length == 0) return;

            byte[] bytes = resultTex.EncodeToPNG();
            File.WriteAllBytes(pingPath, bytes);
            AssetDatabase.ImportAsset(pingPath);
            AssetDatabase.Refresh();
            DestroyImmediate(resultTex);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(pingPath, typeof(Texture)));
            Debug.Log("Render Image saved to: " + fullPath + " with scale: " + scaleSlider +
                      " (it can be changed in Window -> AllIn1VfxWindow)");
        }

        private string GetNewValidPath(string path, int i = 1)
        {
            int number = i;
            string newPath = path + "_" + number.ToString();
            string fullPath = newPath + ".png";
            if(System.IO.File.Exists(fullPath))
            {
                number++;
                fullPath = GetNewValidPath(path, number);
            }

            return fullPath;
        }

        public void AddHelperAndPlaceUnderAll1VfxMainComponent()
        {
            AllIn1ParticleHelperComponent psHelper = GetComponent<AllIn1ParticleHelperComponent>();
            if(psHelper != null) return;
            psHelper = gameObject.AddComponent<AllIn1ParticleHelperComponent>();
            Component[] components = GetComponents(typeof(Component));
            int all1ComponentIndex = -1;
            bool rendererHasAppeared = false;
            for(int i = 0; i < components.Length; i++)
            {
                if(components[i].GetType().FullName.Contains("ParticleSystemRenderer")) rendererHasAppeared = true;
                if(components[i].GetType().FullName.Contains("AllIn1VfxComponent"))
                {
                    all1ComponentIndex = i;
                    break;
                }
            }

            if(all1ComponentIndex <= -1) return;
            int upTimes = components.Length - 2 - all1ComponentIndex;
            if(!rendererHasAppeared) upTimes -= 1; //Takes into account invisible particle Renderer component
            if(upTimes > 0)
                for(int i = 0; i < upTimes; i++)
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(psHelper);
        }

        [ContextMenu("Move component to the top of Inspector")]
        public void MoveComponentToTheTop()
        {
            Component[] components = GetComponents(typeof(Component));
            int upTimes = components.Length - 2;
            if(upTimes > 0)
                for(int i = 0; i < upTimes; i++)
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
        }

        [ContextMenu("Delete all All1Vfx Components in Hierarchy")]
        private void DeleteAllParticleHelpersInHierarchy()
        {
            StartCoroutine(DeleteAllChildAll1VfxComponentsCR());
            StartCoroutine(DelayedDestroyCR());
        }

        [ContextMenu("Delete child All1Vfx Components")]
        private void DeleteChildParticleHelpers()
        {
            StartCoroutine(DeleteAllChildAll1VfxComponentsCR());
        }

        private IEnumerator DelayedDestroyCR()
        {
            yield return null;
            DestroyImmediate(this);
        }

        private IEnumerator DeleteAllChildAll1VfxComponentsCR()
        {
            yield return null;
            AllIn1VfxComponent[] helpers = gameObject.GetComponentsInChildren<AllIn1VfxComponent>();
            for(int i = 0; i < helpers.Length; i++)
                if(!helpers[i].gameObject.Equals(gameObject))
                    DestroyImmediate(helpers[i]);
        }
#endif
    }
}