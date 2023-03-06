#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[CanEditMultipleObjects]
public class AllIn1VfxCustomMaterialEditor : ShaderGUI
{
    private Material targetMat;
    private UnityEngine.Rendering.BlendMode srcMode, dstMode;
    private UnityEngine.Rendering.CompareFunction zTestMode;
    private UnityEngine.Rendering.CullMode cullMode;
    private UnityEngine.Rendering.ColorWriteMask colorMask;

    private Material originalMaterialCopy;
    private MaterialEditor matEditor;
    private MaterialProperty[] matProperties;
    private uint[] materialDrawers = new uint[] { 1, 2, 4, 8, 16, 32 };
    bool[] currEnabledDrawers;
    private const uint advancedConfigDrawer = 0;
    private const uint advancedShapeBlendDrawer = 1;
    private const uint mainShapeDrawer = 2;
    private const uint colorFxShapeDrawer = 3;
    private const uint alphaFxShapeDrawer = 4;
    private const uint uvFxShapeDrawer = 5;

    private enum RenderingPreset
    {
        Transparent,
        Additive,
        SoftAdditive,
        BlendAdd,
        Opaque,
        Custom
    }

    private RenderingPreset renderPreset;

    private string[] oldKeyWords;
    private GUIStyle propertiesStyle, bigLabel, smallLabel = new GUIStyle(), toggleButtonStyle, centeredTextStyle, smallTextStyle, smallBoldLabel;
    private const int bigFontSize = 16;
    private float tempValue;
    bool shape2On = false, shape3On = false, isPs = false;
    int effectCount = 1, shape1FirstProperty, shape2FirstProperty, shape3FirstProperty;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        matEditor = materialEditor;
        matProperties = properties;
        targetMat = materialEditor.target as Material;
        oldKeyWords = targetMat.shaderKeywords;
        effectCount = 1;
        currEnabledDrawers = new bool[materialDrawers.Length];
        uint iniDrawers = (uint)ShaderGUI.FindProperty("_EditorDrawers", matProperties).floatValue;
        for(int i = 0; i < materialDrawers.Length; i++) currEnabledDrawers[i] = (materialDrawers[i] & iniDrawers) > 0;

        propertiesStyle = new GUIStyle(EditorStyles.helpBox);
        propertiesStyle.margin = new RectOffset(0, 0, 0, 0);
        bigLabel = new GUIStyle(EditorStyles.boldLabel) { fontSize = bigFontSize };
        smallLabel.fontStyle = FontStyle.Bold;
        toggleButtonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, richText = true };
        centeredTextStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter, richText = true, fontSize = 13 };
        smallTextStyle = new GUIStyle(GUI.skin.button) { fontSize = 10 };
        smallBoldLabel = new GUIStyle(EditorStyles.boldLabel) { fontSize = 11 };
        if(Selection.activeGameObject != null) isPs = Selection.activeGameObject.GetComponent<ParticleSystem>() != null;

        GUILayout.Label("Configuration", bigLabel);

        GUILayout.Label("Presets:", smallBoldLabel);
        RenderingMode();

        SetLabelAndFieldWidth(178, 75);
        currEnabledDrawers[advancedConfigDrawer] = GUILayout.Toggle(currEnabledDrawers[advancedConfigDrawer], new GUIContent("<size=12>Show Advanced Configuration</size>"), toggleButtonStyle);
        if(currEnabledDrawers[advancedConfigDrawer])
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            Blending();
            DrawLine(Color.grey, 1, 3);
            ZWrite();
            DrawLine(Color.grey, 1, 3);
            ZTest();
            DrawLine(Color.grey, 1, 3);
            Culling();
            DrawLine(Color.grey, 1, 3);
            ColorMask();
            DrawLine(Color.grey, 1, 3);
            DrawProperty(9);
            DrawLine(Color.grey, 1, 3);
            Fog("Use Unity Fog", "FOG_ON");
            DrawLine(Color.grey, 1, 3);
            EditorGUIUtility.labelWidth = 140;
            materialEditor.EnableInstancingField();
            DrawLine(Color.grey, 1, 3);
            materialEditor.RenderQueueField();
            EditorGUILayout.EndVertical();
        }

        SectionSeparatorAndHeader("Shape Properties");
        for(int i = 7; i <= 8; i++) DrawProperty(i);
        EditorGUILayout.Separator();
        currEnabledDrawers[mainShapeDrawer] = GUILayout.Toggle(currEnabledDrawers[mainShapeDrawer], new GUIContent("<size=12>Show Shape Configuration</size>"), toggleButtonStyle);
        if(currEnabledDrawers[mainShapeDrawer])
        {
            shape1FirstProperty = 11;
            shape2FirstProperty = 23;
            shape3FirstProperty = 35;
            ShapeProperties(1, shape1FirstProperty);
            EditorGUILayout.Separator();
            ShapeProperties(2, shape2FirstProperty, true);
            EditorGUILayout.Separator();
            if(shape2On) ShapeProperties(3, shape3FirstProperty, true);
            if(shape2On) ShapeResult();
        }
        else
        {
            ShapeProperties(2, 23, true, true);
            if(shape2On) ShapeProperties(3, 35, true, true);
        }

        SectionSeparatorAndHeader("Color Effects");
        currEnabledDrawers[colorFxShapeDrawer] = GUILayout.Toggle(currEnabledDrawers[colorFxShapeDrawer], new GUIContent("Show Color Effects"), toggleButtonStyle);
        if(currEnabledDrawers[colorFxShapeDrawer])
        {
            Glow("Glow", "GLOW_ON");
            ColorRamp("Color Ramp", "COLORRAMP_ON", "COLORRAMPGRAD_ON");
            GenericEffect("Color Grading", "COLORGRADING_ON", 125, 128);
            GenericEffect("Hue Shift and Saturation", "HSV_ON", 71, 73);
            GenericEffect("Fresnel / Rim Color", "RIM_ON", 135, 141);
            GenericEffect("Intersection Glow", "DEPTHGLOW_ON", 64, 68);
            GenericEffect("Posterize", "POSTERIZE_ON", 149, 149);
            GenericEffect("Backface Tint", "BACKFACETINT_ON", 82, 83);
            FakeLightAndShadow("Fake Light And Shadow", "LIGHTANDSHADOW_ON", 144, 148);
            GenericEffect("Shape 1 Mask", "SHAPE1MASK_ON", 142, 143, true, new string[]
            {
                "It will only have a visual effect if shape 2 or 3 is enabled",
                "Use to avoid shape 1 from being affected by distortions and other shapes"
            });
            ScreenDistort("Screen Distortion", "SCREENDISTORTION_ON", 103, 107);
        }

        SectionSeparatorAndHeader("Alpha Effects");
        currEnabledDrawers[alphaFxShapeDrawer] = GUILayout.Toggle(currEnabledDrawers[alphaFxShapeDrawer], new GUIContent("Show Alpha Effects"), toggleButtonStyle);
        if(currEnabledDrawers[alphaFxShapeDrawer])
        {
            GenericEffect("Alpha Mask", "MASK_ON", 69, 70, true);
            FadeDissolve("Fade From Noise Texture (Dissolve)", "FADE_ON", 115, 124);
            AlphaFade("Fade From Final Shape (Procedural Dissolve)", "ALPHAFADE_ON", 55, 57);
            GenericEffect("Soft Particles / Intersection Fade", "SOFTPART_ON", 47, 47, true);
            CameraDistFade("Camera Distance Fade", "CAMDISTFADE_ON", 129, 131);
            AlphaSmoothstep("Alpha Remap", "ALPHASMOOTHSTEP_ON", 53, 54);
            GenericEffect("Alpha Cutoff", "ALPHACUTOFF_ON", 52, 52, true, new string[] { "Use this to clip transparent pixels and reduce overdraw" });
        }

        SectionSeparatorAndHeader("UV and Vertex Effects");
        currEnabledDrawers[uvFxShapeDrawer] = GUILayout.Toggle(currEnabledDrawers[uvFxShapeDrawer], new GUIContent("Show UV Effects"), toggleButtonStyle);
        if(currEnabledDrawers[uvFxShapeDrawer])
        {
            GenericEffect("Global Distortion", "DISTORT_ON", 78, 81);
            PolarUvs("Global Polar Coordinates", "POLARUV_ON");
            if(isPs || oldKeyWords.Contains("SHAPEWEIGHTS_ON")) ShapeWeightVertexStream("Shape Weights Custom Stream", "SHAPEWEIGHTS_ON", 156);
            if(isPs || oldKeyWords.Contains("OFFSETSTREAM_ON")) OffsetVertexStream("Texture Offset Custom Stream", "OFFSETSTREAM_ON", 100);
            ShapeTextureOffset("Shape Texture Offset", "SHAPETEXOFFSET_ON", 74);
            GenericEffect("Global Texture Scroll", "TEXTURESCROLL_ON", 108, 109);
            GenericEffect("Twist", "TWISTUV_ON", 94, 97);
            GenericEffect("Wave", "WAVEUV_ON", 87, 91);
            GenericEffect("Round Wave", "ROUNDWAVEUV_ON", 92, 93);
            GenericEffect("Hand Drawn", "DOODLE_ON", 98, 99);
            GenericEffect("Pixelate", "PIXELATE_ON", 77, 77, true, new string[] { "Looks bad with distortion effects" });
            TrailWidth("Trail Width", "TRAILWIDTH_ON", 58, 59);
            GenericEffect("Shake", "SHAKEUV_ON", 84, 86);
            GenericEffect("Vertex Offset", "VERTOFFSET_ON", 110, 114);
        }

        SetAndSaveEnabledDrawers(iniDrawers);
    }

    private void SetAndSaveEnabledDrawers(uint iniDrawers)
    {
        uint currDrawers = 0;
        for(int i = 0; i < currEnabledDrawers.Length; i++)
        {
            if(currEnabledDrawers[i]) currDrawers |= materialDrawers[i];
        }

        if(iniDrawers != currDrawers) ShaderGUI.FindProperty("_EditorDrawers", matProperties).floatValue = currDrawers;
    }

    private void SectionSeparatorAndHeader(string headerText)
    {
        EditorGUILayout.Separator();
        DrawLine(Color.grey, 1, 3);
        GUILayout.Label(headerText, bigLabel);
    }

    private void ShapeResult()
    {
        EditorGUILayout.Separator();
        GUILayout.Label("Shape Result:", smallBoldLabel);

        currEnabledDrawers[advancedShapeBlendDrawer] = GUILayout.Toggle(currEnabledDrawers[advancedShapeBlendDrawer], new GUIContent("Show Shape Combination Options"), toggleButtonStyle);
        if(currEnabledDrawers[advancedShapeBlendDrawer])
        {
            EditorGUILayout.BeginVertical(propertiesStyle);

            bool splitColor = DrawEffectSubKeywordToggle("Split Color And Alpha?", "SPLITRGBA_ON");
            bool addResults = DrawEffectSubKeywordToggle("Add Shape Results?", "SHAPEADD_ON");
            char operationChar = '*';
            if(addResults) operationChar = '+';

            if(splitColor)
            {
                GUILayout.Label("Color:", smallBoldLabel);
                DrawProperty(21);
                DrawProperty(33);
                if(shape3On) DrawProperty(45);

                GUILayout.Label("Alpha:", smallBoldLabel);
                DrawProperty(22);
                DrawProperty(34);
                if(shape3On) DrawProperty(46);

                EditorGUILayout.Separator();
                GUILayout.Label("Final Shape Calculation:", smallBoldLabel);
                if(shape3On)
                {
                    GUILayout.Label("RGB = ((Sh1.rgb * " + matProperties[21].floatValue + ") " + operationChar + " (Sh2.rgb *  " + matProperties[33].floatValue
                                    + ")) " + operationChar + " (Sh3.rgb * " + matProperties[45].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    if(addResults)
                        GUILayout.Label("A = max(max(Sh1.a * " + matProperties[22].floatValue + " , " + "Sh2.a *  " + matProperties[34].floatValue
                                        + "), Sh3.a * " + matProperties[46].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    else
                        GUILayout.Label("A = ((Sh1.a * " + matProperties[22].floatValue + ") " + operationChar + " (Sh2.a *  " + matProperties[34].floatValue
                                        + ")) " + operationChar + " (Sh3.a * " + matProperties[46].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                }
                else
                {
                    GUILayout.Label("RGB = (Sh1.rgb * " + matProperties[21].floatValue + ") " + operationChar + " (Sh2.rgb *  " + matProperties[33].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    if(addResults) GUILayout.Label("A = max(Sh1.a * " + matProperties[22].floatValue + " , Sh2.a *  " + matProperties[34].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    else GUILayout.Label("A = (Sh1.a * " + matProperties[22].floatValue + ") " + operationChar + " (Sh2.a *  " + matProperties[34].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                }
            }
            else
            {
                GUILayout.Label("Shape weights:", smallBoldLabel);
                DrawProperty(21);
                DrawProperty(33);
                if(shape3On) DrawProperty(45);
                matProperties[22].floatValue = matProperties[21].floatValue;
                matProperties[34].floatValue = matProperties[33].floatValue;
                matProperties[46].floatValue = matProperties[45].floatValue;

                EditorGUILayout.Separator();
                GUILayout.Label("Final Shape Calculation:", smallBoldLabel);
                if(shape3On)
                {
                    if(!addResults)
                        GUILayout.Label("Result = ((Shape1 * " + matProperties[21].floatValue + ") " + operationChar + " (Shape2 *  " + matProperties[33].floatValue
                                        + ")) " + operationChar + " (Shape3 * " + matProperties[45].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    else
                    {
                        GUILayout.Label("RGB = ((Sh1.rgb * " + matProperties[21].floatValue + ") " + operationChar + " (Sh2.rgb *  " + matProperties[33].floatValue
                                        + ")) " + operationChar + " (Sh3.rgb * " + matProperties[45].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                        GUILayout.Label("A = max(max(Sh1.a * " + matProperties[22].floatValue + " , " + "Sh2.a *  " + matProperties[34].floatValue
                                        + "), Sh3.a * " + matProperties[46].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    }
                }
                else
                {
                    if(!addResults) GUILayout.Label("Result = (Shape1 * " + matProperties[21].floatValue + ") " + operationChar + " (Shape2 *  " + matProperties[33].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    else
                    {
                        GUILayout.Label("RGB = (Sh1.rgb * " + matProperties[21].floatValue + ") " + operationChar + " (Sh2.rgb *  " + matProperties[33].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                        GUILayout.Label("A = max(Sh1.a * " + matProperties[22].floatValue + " , Sh2.a *  " + matProperties[34].floatValue + ")", smallBoldLabel, GUILayout.Height(15));
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }
    }

    private void RenderingMode()
    {
        EditorGUILayout.BeginVertical(propertiesStyle);
        {
            MaterialProperty renderM = matProperties[0];
            tempValue = renderM.floatValue;
            SetLabelAndFieldWidth(89, 75);
            renderPreset = (RenderingPreset)renderM.floatValue;

            if(renderPreset == RenderingPreset.Custom)
                renderPreset = (RenderingPreset)GUILayout.SelectionGrid((int)renderPreset, new string[] { "Transparent", "Additive", "Soft Add", "Blend Add / Premultiply", "Opaque", "Custom" }, 3, smallTextStyle);
            else
                renderPreset = (RenderingPreset)GUILayout.SelectionGrid((int)renderPreset, new string[] { "Transparent", "Additive", "Soft Add", "Blend Add / Premultiply", "Opaque" }, 3, smallTextStyle);


            renderM.floatValue = (float)(renderPreset);
            if(tempValue != renderM.floatValue && !Application.isPlaying)
            {
                switch(renderPreset)
                {
                    case RenderingPreset.Transparent:
                        targetMat.SetOverrideTag("RenderType", "Transparent");
                        targetMat.SetInt("_SrcMode", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        targetMat.SetInt("_DstMode", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        if(targetMat.renderQueue < 2050) targetMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                        targetMat.DisableKeyword("ADDITIVECONFIG_ON");
                        targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
                        targetMat.DisableKeyword("PREMULTIPLYCOLOR_ON");
                        break;
                    case RenderingPreset.Additive:
                        targetMat.SetOverrideTag("RenderType", "Transparent");
                        targetMat.SetInt("_SrcMode", (int)UnityEngine.Rendering.BlendMode.One);
                        targetMat.SetInt("_DstMode", (int)UnityEngine.Rendering.BlendMode.One);
                        if(targetMat.renderQueue < 2050) targetMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                        targetMat.EnableKeyword("ADDITIVECONFIG_ON");
                        targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
                        targetMat.DisableKeyword("PREMULTIPLYCOLOR_ON");
                        break;
                    case RenderingPreset.SoftAdditive:
                        targetMat.SetOverrideTag("RenderType", "Transparent");
                        targetMat.SetInt("_SrcMode", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
                        targetMat.SetInt("_DstMode", (int)UnityEngine.Rendering.BlendMode.One);
                        if(targetMat.renderQueue < 2050) targetMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                        targetMat.EnableKeyword("ADDITIVECONFIG_ON");
                        targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
                        targetMat.DisableKeyword("PREMULTIPLYCOLOR_ON");
                        break;
                    case RenderingPreset.BlendAdd:
                        targetMat.SetOverrideTag("RenderType", "Transparent");
                        targetMat.SetInt("_SrcMode", (int)UnityEngine.Rendering.BlendMode.One);
                        targetMat.SetInt("_DstMode", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        if(targetMat.renderQueue < 2050) targetMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                        targetMat.EnableKeyword("ADDITIVECONFIG_ON");
                        targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
                        targetMat.EnableKeyword("PREMULTIPLYCOLOR_ON");
                        break;
                    case RenderingPreset.Opaque:
                        targetMat.SetOverrideTag("RenderType", "");
                        targetMat.SetInt("_SrcMode", (int)UnityEngine.Rendering.BlendMode.One);
                        targetMat.SetInt("_DstMode", (int)UnityEngine.Rendering.BlendMode.Zero);
                        if(targetMat.renderQueue >= 2050) targetMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                        targetMat.SetInt("_ZWrite", 1);
                        targetMat.DisableKeyword("ADDITIVECONFIG_ON");
                        targetMat.DisableKeyword("PREMULTIPLYALPHA_ON");
                        targetMat.DisableKeyword("PREMULTIPLYCOLOR_ON");
                        break;
                }

                Save();
                oldKeyWords = targetMat.shaderKeywords;
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void Blending()
    {
        MaterialProperty srcM = matProperties[1];
        MaterialProperty dstM = matProperties[2];
        SetLabelAndFieldWidth(15, 15);
        EditorGUILayout.LabelField("Alpha Blending Modes:");
        EditorGUILayout.BeginHorizontal();
        {
            srcMode = (UnityEngine.Rendering.BlendMode)srcM.floatValue;
            dstMode = (UnityEngine.Rendering.BlendMode)dstM.floatValue;
            SetLabelAndFieldWidth(25, 50);
            tempValue = (float)(srcMode);
            srcMode = (UnityEngine.Rendering.BlendMode)EditorGUILayout.EnumPopup("Src", srcMode);
            if(tempValue != (float)(srcMode) && !Application.isPlaying) SaveAndSetCustomConfig();
            EditorGUILayout.Space();
            tempValue = (float)(dstMode);
            dstMode = (UnityEngine.Rendering.BlendMode)EditorGUILayout.EnumPopup("Dst", dstMode);
            if(tempValue != (float)(dstMode) && !Application.isPlaying) SaveAndSetCustomConfig();
            srcM.floatValue = (float)(srcMode);
            dstM.floatValue = (float)(dstMode);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        SetLabelAndFieldWidth(137, 75);
        DrawEffectSubKeywordToggle("Additive configuration?", "ADDITIVECONFIG_ON", true);

        SetLabelAndFieldWidth(112, 75);
        DrawEffectSubKeywordToggle("Premultiply Alpha?", "PREMULTIPLYALPHA_ON", true);
        DrawEffectSubKeywordToggle("Premultiply Color?", "PREMULTIPLYCOLOR_ON", true);
    }

    private void ZWrite()
    {
        MaterialProperty zWrite = ShaderGUI.FindProperty("_ZWrite", matProperties);
        bool toggle = zWrite.floatValue > 0.9f ? true : false;
        EditorGUILayout.BeginHorizontal();
        {
            SetLabelAndFieldWidth(89, 2);
            tempValue = zWrite.floatValue;
            toggle = GUILayout.Toggle(toggle, new GUIContent("Enable Z Write"));
            if(toggle) zWrite.floatValue = 1.0f;
            else zWrite.floatValue = 0.0f;
            if(tempValue != zWrite.floatValue && !Application.isPlaying) SaveAndSetCustomConfig();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ZTest()
    {
        MaterialProperty zTestM = ShaderGUI.FindProperty("_ZTestMode", matProperties);
        tempValue = zTestM.floatValue;
        SetLabelAndFieldWidth(72, 75);
        zTestMode = (UnityEngine.Rendering.CompareFunction)zTestM.floatValue;
        zTestMode = (UnityEngine.Rendering.CompareFunction)EditorGUILayout.EnumPopup("Z TestMode", zTestMode);
        zTestM.floatValue = (float)(zTestMode);
        if(tempValue != zTestM.floatValue && !Application.isPlaying) SaveAndSetCustomConfig();
    }

    private void Culling()
    {
        MaterialProperty cullO = matProperties[3];
        tempValue = cullO.floatValue;
        SetLabelAndFieldWidth(83, 35);
        cullMode = (UnityEngine.Rendering.CullMode)cullO.floatValue;
        cullMode = (UnityEngine.Rendering.CullMode)EditorGUILayout.EnumPopup("Culling Mode", cullMode);
        cullO.floatValue = (float)(cullMode);
        if(tempValue != cullO.floatValue && !Application.isPlaying) SaveAndSetCustomConfig();
    }

    private void ColorMask()
    {
        MaterialProperty colorM = matProperties[6];
        tempValue = colorM.floatValue;
        SetLabelAndFieldWidth(105, 35);
        colorMask = (UnityEngine.Rendering.ColorWriteMask)colorM.floatValue;
        colorMask = (UnityEngine.Rendering.ColorWriteMask)EditorGUILayout.EnumPopup("Color Write Mask", colorMask);
        colorM.floatValue = (float)(colorMask);
        if(tempValue != colorM.floatValue && !Application.isPlaying) SaveAndSetCustomConfig();
    }

    private void Fog(string inspector, string keyword)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        SetLabelAndFieldWidth(87, 50);
        DrawEffectSubKeywordToggle(inspector, keyword, true);
    }

    private void ShapeProperties(int shapeNumber, int firstProperty, bool onlyIfKeyword = false, bool getIfEnabledAndReturn = false)
    {
        string shapeText = "Shape " + shapeNumber;
        string shapeKeyword = "SHAPE" + shapeNumber;
        bool toggle = false;
        if(shapeNumber == 2) shape2On = false;
        else if(shapeNumber == 3) shape3On = false;

        if(onlyIfKeyword)
        {
            bool ini = oldKeyWords.Contains(shapeKeyword + "_ON");
            if(ini)
            {
                if(shapeNumber == 2) shape2On = true;
                else if(shapeNumber == 3) shape3On = true;
            }

            if(getIfEnabledAndReturn) return;
            toggle = ini;
            toggle = GUILayout.Toggle(toggle, new GUIContent("Use Shape " + shapeNumber + "?"), toggleButtonStyle);
            if(ini != toggle)
            {
                Save();
                if(toggle) targetMat.EnableKeyword(shapeKeyword + "_ON");
                else
                {
                    targetMat.DisableKeyword(shapeKeyword + "_ON");
                    if(shapeNumber == 2)
                    {
                        targetMat.DisableKeyword("SHAPE3_ON");
                        targetMat.DisableKeyword("SHAPE2SCREENUV_ON");
                        targetMat.DisableKeyword("SHAPE3SCREENUV_ON");
                        SetShaderBasedOnEffectsAndPipeline();
                    }
                    else if(shapeNumber == 3)
                    {
                        SetShaderBasedOnEffectsAndPipeline();
                        targetMat.DisableKeyword("SHAPE3SCREENUV_ON");
                    }
                }

                return;
            }
        }

        if(!onlyIfKeyword || toggle)
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            GUILayout.Label(shapeText + ":", smallBoldLabel);
            for(int i = firstProperty; i <= firstProperty + 3; i++) DrawProperty(i);
            string nextShapeKeyword = shapeKeyword + "CONTRAST_ON";
            GenericEffect(shapeText + " Contrast", nextShapeKeyword, firstProperty + 4, firstProperty + 5, false);
            nextShapeKeyword = shapeKeyword + "DISTORT_ON";
            GenericEffect(shapeText + " Distortion", nextShapeKeyword, firstProperty + 6, firstProperty + 9, false);
            nextShapeKeyword = shapeKeyword + "ROTATE_ON";
            int shapeRotationPropertyIndex = 150 + (shapeNumber - 1) * 2;
            GenericEffect("Shape " + shapeNumber + " Rotation", nextShapeKeyword, shapeRotationPropertyIndex, shapeRotationPropertyIndex + 1, false);
            nextShapeKeyword = shapeKeyword + "SHAPECOLOR_ON";
            GenericEffect("Shape " + shapeNumber + " RGB is Shape Color, Red Channel Is Alpha", nextShapeKeyword, -1, -1, false);
            nextShapeKeyword = shapeKeyword + "SCREENUV_ON";
            int shapeSSUvScalePropertyIndex = 132 - 1 + shapeNumber; //- 1 from shader index to account for shape number
            GenericEffect("Shape " + shapeNumber + " Screen Position UVs?", nextShapeKeyword, shapeSSUvScalePropertyIndex, shapeSSUvScalePropertyIndex, false);

            nextShapeKeyword = "SHAPEDEBUG_ON";
            int shapeDebugPropertyIndex = matProperties.Length - 1;
            bool ini = oldKeyWords.Contains(nextShapeKeyword) && matProperties[shapeDebugPropertyIndex].floatValue < shapeNumber + 0.5f && matProperties[shapeDebugPropertyIndex].floatValue > shapeNumber - 0.5f;
            bool toggle2 = ini;
            toggle2 = EditorGUILayout.BeginToggleGroup(shapeText + " Debug", toggle2);
            EditorGUILayout.EndToggleGroup();
            if(ini != toggle2)
            {
                Save();
                if(toggle2) targetMat.EnableKeyword(nextShapeKeyword);
                else targetMat.DisableKeyword(nextShapeKeyword);
                matProperties[shapeDebugPropertyIndex].floatValue = shapeNumber;
            }

            if(shapeNumber == 1)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if(oldKeyWords.Contains("SHAPE2_ON"))
                    {
                        if(GUILayout.Button("Copy Shape 2 Properties", GUILayout.Width(200)))
                        {
                            CopyShapePropertiesAndKeywords(shape1FirstProperty, shape2FirstProperty, 1, 2);
                        }
                    }

                    if(oldKeyWords.Contains("SHAPE3_ON"))
                    {
                        if(GUILayout.Button("Copy Shape 3 Properties", GUILayout.Width(200)))
                        {
                            CopyShapePropertiesAndKeywords(shape1FirstProperty, shape3FirstProperty, 1, 3);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else if(shapeNumber == 2)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if(GUILayout.Button("Copy Shape 1 Properties", GUILayout.Width(200)))
                    {
                        CopyShapePropertiesAndKeywords(shape2FirstProperty, shape1FirstProperty, 2, 1);
                    }

                    if(oldKeyWords.Contains("SHAPE3_ON"))
                    {
                        if(GUILayout.Button("Copy Shape 3 Properties", GUILayout.Width(200)))
                        {
                            CopyShapePropertiesAndKeywords(shape2FirstProperty, shape3FirstProperty, 2, 3);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else if(shapeNumber == 3)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if(GUILayout.Button("Copy Shape 1 Properties", GUILayout.Width(200)))
                    {
                        CopyShapePropertiesAndKeywords(shape3FirstProperty, shape1FirstProperty, 3, 1);
                    }

                    if(GUILayout.Button("Copy Shape 2 Properties", GUILayout.Width(200)))
                    {
                        CopyShapePropertiesAndKeywords(shape3FirstProperty, shape2FirstProperty, 3, 2);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
    }

    private void CopyShapePropertiesAndKeywords(int shapeToFirstProperty, int shapeFromFirstProperty, int shapeToNumber, int shapeFromNumber)
    {
        for(int i = 0; i < 12; i++) CopyShapeProperty(shapeToFirstProperty + i, shapeFromFirstProperty + i);
        CopyShapeProperty(129 + shapeToNumber, 129 + shapeFromNumber);
        if(oldKeyWords.Contains("SHAPE" + shapeFromNumber + "CONTRAST_ON")) targetMat.EnableKeyword("SHAPE" + shapeToNumber + "CONTRAST_ON");
        else targetMat.DisableKeyword("SHAPE" + shapeToNumber + "CONTRAST_ON");
        if(oldKeyWords.Contains("SHAPE" + shapeFromNumber + "DISTORT_ON")) targetMat.EnableKeyword("SHAPE" + shapeToNumber + "DISTORT_ON");
        else targetMat.DisableKeyword("SHAPE" + shapeToNumber + "DISTORT_ON");
        
        if(oldKeyWords.Contains("SHAPE" + shapeFromNumber + "ROTATE_ON")) targetMat.EnableKeyword("SHAPE" + shapeToNumber + "ROTATE_ON");
        else targetMat.DisableKeyword("SHAPE" + shapeToNumber + "ROTATE_ON");
        int rotationOffsetToIndex = 150 + ((shapeToNumber - 1) * 2); //150 is the first rotation property
        int rotationOffsetFromIndex = 150 + ((shapeFromNumber - 1) * 2);
        CopyShapeProperty(rotationOffsetToIndex, rotationOffsetFromIndex);
        CopyShapeProperty(rotationOffsetToIndex + 1, rotationOffsetFromIndex + 1); //Copy rotation speed
        
        if(oldKeyWords.Contains("SHAPE" + shapeFromNumber + "SHAPECOLOR_ON")) targetMat.EnableKeyword("SHAPE" + shapeToNumber + "SHAPECOLOR_ON");
        else targetMat.DisableKeyword("SHAPE" + shapeToNumber + "SHAPECOLOR_ON");
        if(oldKeyWords.Contains("SHAPE" + shapeFromNumber + "SCREENUV_ON")) targetMat.EnableKeyword("SHAPE" + shapeToNumber + "SCREENUV_ON");
        else targetMat.DisableKeyword("SHAPE" + shapeToNumber + "SCREENUV_ON");
        SetShaderBasedOnEffectsAndPipeline();
    }

    private void CopyShapeProperty(int propertyTo, int propertyFrom)
    {
        if(matProperties[propertyTo].type == MaterialProperty.PropType.Float || matProperties[propertyTo].type == MaterialProperty.PropType.Range)
        {
            matProperties[propertyTo].floatValue = matProperties[propertyFrom].floatValue;
        }
        else if(matProperties[propertyTo].type == MaterialProperty.PropType.Vector)
        {
            matProperties[propertyTo].vectorValue = matProperties[propertyFrom].vectorValue;
        }
        else if(matProperties[propertyTo].type == MaterialProperty.PropType.Color)
        {
            matProperties[propertyTo].colorValue = matProperties[propertyFrom].colorValue;
        }
        else if(matProperties[propertyTo].type == MaterialProperty.PropType.Texture)
        {
            matProperties[propertyTo].textureValue = matProperties[propertyFrom].textureValue;
            matProperties[propertyTo].textureScaleAndOffset = matProperties[propertyFrom].textureScaleAndOffset;
        }
    }

    private void GenericEffect(string inspector, string keyword, int first, int last, bool effectCounter = true,
        string[] preMessages = null, int[] extraProperties = null, string[] keywordsToDisable = null)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;

        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        if(effectCounter)
        {
            effectNameLabel.text = effectCount + "." + inspector;
            toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);
            effectCount++;
        }
        else
        {
            effectNameLabel.text = inspector;
            toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);
        }

        if(ini != toggle && !Application.isPlaying)
        {
            if(toggle)
            {
                targetMat.EnableKeyword(keyword);
                if(keywordsToDisable != null)
                    foreach(string effect in keywordsToDisable)
                        targetMat.DisableKeyword(effect);
            }
            else targetMat.DisableKeyword(keyword);

            SetShaderBasedOnEffectsAndPipeline();
            Save();
        }

        if(toggle && first > 0)
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                if(preMessages != null)
                {
                    foreach(string s in preMessages)
                    {
                        GUILayout.Label(s, smallLabel);
                        EditorGUILayout.Space();
                    }
                }

                for(int i = first; i <= last; i++) DrawProperty(i);
                if(extraProperties != null)
                    foreach(int i in extraProperties)
                        DrawProperty(i);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void Glow(string inspector, string keyword)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawProperty(62);

                EditorGUILayout.Separator();
                if(DrawEffectSubKeywordToggle("Use Glow Mask Tex?", "GLOWTEX_ON")) DrawProperty(63);
                DrawProperty(60);
                DrawProperty(61);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void ColorRamp(string inspector, string keyword, string keyword2)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                bool useEditableGradient = false;
                if(AssetDatabase.Contains(targetMat))
                {
                    useEditableGradient = oldKeyWords.Contains(keyword2);
                    bool gradientTex = useEditableGradient;
                    gradientTex = GUILayout.Toggle(gradientTex, new GUIContent("Use Editable Gradient?"));
                    if(useEditableGradient != gradientTex)
                    {
                        if(!Application.isPlaying) Save();
                        if(gradientTex)
                        {
                            useEditableGradient = true;
                            targetMat.EnableKeyword(keyword2);
                        }
                        else targetMat.DisableKeyword(keyword2);
                    }

                    if(useEditableGradient) matEditor.ShaderProperty(matProperties[50], matProperties[50].displayName);
                }
                else
                {
                    GUILayout.Label("*Save to folder to allow for dynamic Gradient property", smallLabel);
                    EditorGUILayout.Space();
                }

                GUILayout.Label("*Premultiply Color toggle recommended on additive setups", smallLabel);
                EditorGUILayout.Space();

                if(!useEditableGradient) DrawProperty(48);

                DrawProperty(49);
                DrawProperty(51);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AlphaSmoothstep(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawMinMaxSlider("Min And Max Slider", first, last);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void CameraDistFade(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawMinMaxSlider("Min And Max Slider", first, first + 1, 0f, 2000f);
                EditorGUILayout.Space();
                DrawProperty(last);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void AlphaFade(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawEffectSubKeywordToggle("Fade Amount Affects Global Transparency?", "ALPHAFADETRANSPARENCYTOO_ON");
                if(isPs)
                {
                    if(DrawEffectSubKeywordToggle("Fade Amount Driven By Vertex Stream? Custom1.y (See Docs)", "ALPHAFADEINPUTSTREAM_ON"))
                        GUILayout.Label("*Custom Data Auto Setup button on Particle Helper\n component will do the setup for you", smallLabel);
                }

                DrawEffectSubKeywordToggle("Use grayscale as alpha? (Good for Additive configurations)", "ALPHAFADEUSEREDCHANNEL_ON");
                if(DrawEffectSubKeywordToggle("Use Shape1 as fade mask?", "ALPHAFADEUSESHAPE1_ON"))
                {
                    GUILayout.Label("*The effect is using the Shape1 result as the fade mask texture", smallLabel);
                }
                else GUILayout.Label("*The effect is using the global result as the fade mask texture", smallLabel);

                DrawLine(Color.grey, 1, 3);
                for(int i = first; i <= last; i++) DrawProperty(i);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void Blur(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("It will only affect Shape 1", smallLabel);
                EditorGUILayout.Space();
                DrawEffectSubKeywordToggle("Is Blur HD? (Performance Intensive)", "BLURISHD_ON");
                for(int i = first; i <= last; i++) DrawProperty(i);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void OffsetVertexStream(string inspector, string keyword, int first)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("*Use Particle System Custom Data Custom2.xy (See Docs)", smallLabel);
                EditorGUILayout.Space();
                GUILayout.Label("*Custom Data Auto Setup button on Particle Helper\n component will do the setup for you", smallLabel);
                DrawProperty(first);
                if(shape2On) DrawProperty(first + 1);
                if(shape3On) DrawProperty(first + 2);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void ShapeWeightVertexStream(string inspector, string keyword, int first)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                if(!shape2On && !shape3On)
                {
                    GUILayout.Label("*THIS EFFECT NEEDS AT LEAST 2 ACTIVE SHAPES", smallLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }

                GUILayout.Label("*Use Particle System Custom Data Custom2.z (See Docs)", smallLabel);
                EditorGUILayout.Space();
                GUILayout.Label("*Custom Data Auto Setup button on Particle Helper\n component will do the setup for you", smallLabel);
                EditorGUILayout.Space();
                GUILayout.Label("*Offset is added to shape weight", smallLabel);
                if(shape2On || shape3On) DrawProperty(first);
                if(shape2On) DrawProperty(first + 1);
                if(shape3On) DrawProperty(first + 2);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void ShapeTextureOffset(string inspector, string keyword, int first)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("*Uses Random Seed to offset Shape textures", smallLabel);
                DrawProperty(first);
                if(shape2On) DrawProperty(first + 1);
                if(shape3On) DrawProperty(first + 2);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void ScreenDistort(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("*This effect is performant intensive", smallLabel);
                GUILayout.Label("*Use the Transparent Preset", smallLabel);
                EditorGUILayout.Space();
                DrawEffectSubKeywordToggle("Distort only objects behind? (Less performant)", "DISTORTONLYBACK_ON");
                if(!DrawEffectSubKeywordToggle("Use Global Color as distortion mask?", "DISTORTUSECOL_ON"))
                {
                    DrawProperty(first);
                    for(int i = last - 1; i <= last; i++) DrawProperty(i);
                }

                for(int i = first + 1; i <= last - 2; i++) DrawProperty(i);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void FadeDissolve(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                DrawEffectSubKeywordToggle("Fade Amount Affects Global Transparency?", "ALPHAFADETRANSPARENCYTOO_ON");
                if(isPs)
                {
                    if(DrawEffectSubKeywordToggle("Fade Amount Driven By Vertex Stream? Custom1.y (See Docs)", "ALPHAFADEINPUTSTREAM_ON"))
                        GUILayout.Label("*Custom Data Auto Setup button on Particle Helper\n component will do the setup for you", smallLabel);
                }

                DrawLine(Color.grey, 1, 3);
                for(int i = first; i <= first + 5; i++) DrawProperty(i);
                DrawLine(Color.grey, 1, 3);
                if(DrawEffectSubKeywordToggle("Use Fade Burn Color?", "FADEBURN_ON"))
                    for(int i = first + 6; i <= last; i++)
                        DrawProperty(i);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void PolarUvs(string inspector, string keyword)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword, "POLARUVDISTORT_ON"))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("*Disable Generate Mip Maps on textures \nto avoid transparent line visual artifact", smallLabel);
                EditorGUILayout.Space();
                DrawEffectSubKeywordToggle("Polar Coords affects Distortion textures?", "POLARUVDISTORT_ON");
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void TrailWidth(string inspector, string keyword, int powerProperty, int gradientProperty)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("Used for a more accurate width along trail (see Docs)", smallLabel);
                EditorGUILayout.Space();
                GUILayout.Label("White is max width, black is 0 width", smallLabel);
                EditorGUILayout.Space();
                DrawProperty(powerProperty);
                matEditor.ShaderProperty(matProperties[gradientProperty], matProperties[gradientProperty].displayName);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private void FakeLightAndShadow(string inspector, string keyword, int first, int last)
    {
        if(SpecialCaseEffectBodyToggle(inspector, keyword))
        {
            EditorGUILayout.BeginVertical(propertiesStyle);
            {
                GUILayout.Label("You'll need AllIn1VfxFakeLightDirSetter.cs in the scene (see docs)", smallLabel);
                for(int i = first; i <= first + 2; i++) DrawProperty(i);
                DrawMinMaxSlider("Min And Max Slider", last - 1, last);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndToggleGroup();
    }

    private bool DrawEffectSubKeywordToggle(string inspector, string keyword, bool setCustomConfigAfter = false)
    {
        GUIContent propertyLabel = new GUIContent();
        propertyLabel.text = inspector;
        propertyLabel.tooltip = keyword + " (C#)";

        bool ini = oldKeyWords.Contains(keyword);
        bool toggle = ini;
        toggle = GUILayout.Toggle(toggle, propertyLabel);
        if(ini != toggle)
        {
            if(toggle) targetMat.EnableKeyword(keyword);
            else targetMat.DisableKeyword(keyword);

            SetShaderBasedOnEffectsAndPipeline();
            if(!setCustomConfigAfter) Save();
            else SaveAndSetCustomConfig();
        }

        return toggle;
    }

    private bool SpecialCaseEffectBodyToggle(string inspector, string keyword, string extraKeywordToDisable = null)
    {
        bool toggle = oldKeyWords.Contains(keyword);
        bool ini = toggle;
        toggle = SpecialCaseEffectHeaderToggle(inspector, keyword, toggle);

        effectCount++;
        if(ini != toggle && !Application.isPlaying)
        {
            if(toggle) targetMat.EnableKeyword(keyword);
            else
            {
                targetMat.DisableKeyword(keyword);
                if(extraKeywordToDisable != null) targetMat.DisableKeyword(extraKeywordToDisable);
            }

            SetShaderBasedOnEffectsAndPipeline();
            Save();
        }

        return toggle;
    }

    private void SetShaderBasedOnEffectsAndPipeline()
    {
        oldKeyWords = targetMat.shaderKeywords;
        string targetShader = "AllIn1Vfx";

        string pipeline = "Built-In";
        RenderPipelineAsset renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
        if(renderPipelineAsset != null)
        {
            switch(renderPipelineAsset.GetType().Name)
            {
                case"UniversalRenderPipelineAsset":
                    pipeline = "URP";
                    break;
                case"HDRenderPipelineAsset":
                    pipeline = "HDRP";
                    break;
            }
        }

        if(pipeline.Equals("Built-In"))
        {
            if(oldKeyWords.Contains("SCREENDISTORTION_ON")) targetShader = "AllIn1VfxGrabPass";
            else if(oldKeyWords.Contains("FOG_ON") || oldKeyWords.Contains("SHAPE1SCREENUV_ON") || oldKeyWords.Contains("SHAPE2SCREENUV_ON") ||
                    oldKeyWords.Contains("SHAPE3SCREENUV_ON") || oldKeyWords.Contains("SOFTPART_ON") || oldKeyWords.Contains("DEPTHGLOW_ON")) targetShader = "AllIn1VfxBuiltIn";
        }
        else if(pipeline.Equals("URP"))
        {
            targetShader = "AllIn1VfxURP";
        }
        else if(pipeline.Equals("HDRP"))
        {
            targetShader = "AllIn1VfxHDRP";
        }

        if(!targetMat.shader.name.Equals(targetShader))
        {
            int renderingQueue = targetMat.renderQueue;
            targetMat.shader = Resources.Load(targetShader, typeof(Shader)) as Shader;
            targetMat.renderQueue = renderingQueue;
        }
    }

    private bool SpecialCaseEffectHeaderToggle(string inspector, string keyword, bool toggle)
    {
        GUIContent effectNameLabel = new GUIContent();
        effectNameLabel.tooltip = keyword + " (C#)";
        effectNameLabel.text = effectCount + "." + inspector;
        toggle = EditorGUILayout.BeginToggleGroup(effectNameLabel, toggle);
        return toggle;
    }

    private void DrawProperty(int index, bool noReset = false)
    {
        MaterialProperty targetProperty = matProperties[index];

        EditorGUILayout.BeginHorizontal();
        {
            GUIContent propertyLabel = new GUIContent();
            propertyLabel.text = targetProperty.displayName;
            propertyLabel.tooltip = targetProperty.name + " (C#)";

            matEditor.ShaderProperty(targetProperty, propertyLabel);

            if(!noReset)
            {
                GUIContent resetButtonLabel = new GUIContent();
                resetButtonLabel.text = "R";
                resetButtonLabel.tooltip = "Resets to default value";
                if(GUILayout.Button(resetButtonLabel, GUILayout.Width(20))) ResetProperty(targetProperty);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ResetProperty(MaterialProperty targetProperty)
    {
        if(originalMaterialCopy == null) originalMaterialCopy = new Material(targetMat.shader);
        if(targetProperty.type == MaterialProperty.PropType.Float || targetProperty.type == MaterialProperty.PropType.Range)
        {
            targetProperty.floatValue = originalMaterialCopy.GetFloat(targetProperty.name);
        }
        else if(targetProperty.type == MaterialProperty.PropType.Vector)
        {
            targetProperty.vectorValue = originalMaterialCopy.GetVector(targetProperty.name);
        }
        else if(targetProperty.type == MaterialProperty.PropType.Color)
        {
            targetProperty.colorValue = originalMaterialCopy.GetColor(targetProperty.name);
        }
        else if(targetProperty.type == MaterialProperty.PropType.Texture)
        {
            targetProperty.textureValue = originalMaterialCopy.GetTexture(targetProperty.name);
            targetProperty.textureScaleAndOffset = new Vector4(1, 1, 0, 0);
        }
    }

    private void DrawMinMaxSlider(string sliderText, int propMin, int propMax, float valueMin = 0f, float valueMax = 1f)
    {
        DrawProperty(propMin);
        DrawProperty(propMax);
        MaterialProperty matPropMin = matProperties[propMin];
        MaterialProperty matPropMax = matProperties[propMax];
        float stepMin = matPropMin.floatValue;
        float stepMax = matPropMax.floatValue;
        EditorGUILayout.MinMaxSlider(sliderText, ref stepMin, ref stepMax, valueMin, valueMax);
        if(stepMax < stepMin) stepMax = stepMin;
        matPropMin.floatValue = stepMin;
        matPropMax.floatValue = stepMax;
    }

    private static void SetLabelAndFieldWidth(int labelWidth, int fieldWidth)
    {
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUIUtility.fieldWidth = fieldWidth;
    }

    //SetLabelWidthFromText("zTestMode");
    //GUILayout.Label(EditorGUIUtility.labelWidth.ToString());
    //SetFieldWidthFromText("Less Equals");
    //GUILayout.Label(EditorGUIUtility.fieldWidth.ToString());
    private void SetLabelWidthFromText(string text)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(text));
        EditorGUIUtility.labelWidth = textDimensions.x;
        Debug.Log("Label: " + text + "   " + textDimensions.x);
    }

    private void SetFieldWidthFromText(string text)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(text));
        EditorGUIUtility.fieldWidth = textDimensions.x;
        Debug.Log("Field: " + text + "   " + textDimensions.x);
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

    private void SaveAndSetCustomConfig()
    {
        renderPreset = RenderingPreset.Custom;
        MaterialProperty renderM = matProperties[0];
        renderM.floatValue = (float)(renderPreset);
        Save();
    }

    private void Save()
    {
        if(!Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorUtility.SetDirty(targetMat);
    }
}
#endif