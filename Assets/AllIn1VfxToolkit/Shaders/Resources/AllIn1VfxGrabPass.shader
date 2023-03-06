Shader "AllIn1Vfx/AllIn1VfxGrabPass"
{
    Properties
    {
        _RenderingMode("Rendering Mode", float) = 0 // 0
        _SrcMode("SrcMode", float) = 5 // 1
        _DstMode("DstMode", float) = 10 // 2
        _CullingOption("Culling Option", float) = 0 // 3
        _ZWrite("Depth Write", float) = 0.0 // 4
        _ZTestMode("Z Test Mode", float) = 4 // 5
        _ColorMask("Color Write Mask", float) = 15 // 6

        _Alpha("Global Alpha", Range(0, 1)) = 1 //7
        _Color("Global Color", Color) = (1,1,1,1) //8
        
        _TimingSeed("Random Seed", Float) = 0.0 //9
        _EditorDrawers("Editor Drawers", Int) = 60 //10

        _MainTex("Shape1 Texture", 2D) = "white" {} //11
        [HDR] _ShapeColor("Shape1 Color", Color) = (1,1,1,1) //12
        _ShapeXSpeed("Shape1 X Speed", Float) = 0 //13
        _ShapeYSpeed("Shape1 Y Speed", Float) = 0 //14
        _ShapeContrast("Shape1 Contrast", Range (0, 10)) = 1 //15
        _ShapeBrightness("Shape1 Brightness", Range (-1, 1)) = 0 //16
        _ShapeDistortTex("Distortion Texture", 2D) = "black" {} //17
        _ShapeDistortAmount("Distortion Amount", Range(0, 10)) = 0.5 //18
        _ShapeDistortXSpeed("Scroll speed X", Float) = 0.1 //19
        _ShapeDistortYSpeed("Scroll speed Y", Float) = 0.1 //20
        _ShapeColorWeight("Shape1 RGB Weight", Range(0, 5)) = 1 //21
        _ShapeAlphaWeight("Shape1 A Weight", Range(0, 5)) = 1 //22

        _Shape2Tex ("Shape2 Texture", 2D) = "white" {} //23
        [HDR] _Shape2Color("Shape2 Color", Color) = (1,1,1,1)
        _Shape2XSpeed("Shape2 X Speed", Float) = 0
        _Shape2YSpeed("Shape2 Y Speed", Float) = 0
        _Shape2Contrast("Shape2 Contrast", Range (0, 10)) = 1
        _Shape2Brightness("Shape2 Brightness", Range (-1, 1)) = 0
        _Shape2DistortTex("Distortion Texture", 2D) = "black" {}
        _Shape2DistortAmount("Distortion Amount", Range(0,10)) = 0.5
        _Shape2DistortXSpeed("Scroll Speed X", Float) = 0.1
        _Shape2DistortYSpeed("Scroll Speed Y", Float) = 0.1
        _Shape2ColorWeight("Shape2 RGB Weight", Range(0, 5)) = 2
        _Shape2AlphaWeight("Shape2 A Weight", Range(0, 5)) = 2 //34

        _Shape3Tex("Shape3 Texture", 2D) = "white" {} //35
        [HDR] _Shape3Color("Shape3 Color", Color) = (1,1,1,1)
        _Shape3XSpeed("Shape3 X Speed", Float) = 0
        _Shape3YSpeed("Shape3 Y Speed", Float) = 0
        _Shape3Contrast("Shape3 Contrast", Range (0, 10)) = 1
        _Shape3Brightness("Shape3 Brightness", Range (-1, 1)) = 0
        _Shape3DistortTex("Distortion Texture", 2D) = "black" {}
        _Shape3DistortAmount("Distortion Amount", Range(0, 10)) = 0.5
        _Shape3DistortXSpeed("Scroll Speed X", Float) = 0.1
        _Shape3DistortYSpeed("Scroll Speed Y", Float) = 0.1
        _Shape3ColorWeight("Shape3 RGB Weight", Range(0, 5)) = 2
        _Shape3AlphaWeight("Shape3 A Weight", Range(0, 5)) = 2 //46

        _SoftFactor("Soft Particles Factor", Range(0.01, 3.0)) = 0.5 //47

        [NoScaleOffset] _ColorRampTex("Color Ramp Texture", 2D) = "white" {} //48
        _ColorRampLuminosity("Color Ramp luminosity", Range(-1, 1)) = 0 //49
        [AllIn1VfxGradient] _ColorRampTexGradient("Color Ramp Gradient", 2D) = "white" {} //50
        _ColorRampBlend ("Color Ramp Blend", Range(0, 1)) = 1 // 51

        _AlphaCutoffValue("Alpha cutoff value", Range(0, 1)) = 0.25 //52
        _AlphaStepMin("Smoothstep Min", Range(0, 1)) = 0.0 //53
        _AlphaStepMax("Smoothstep Max", Range(0, 1)) = 0.075 //54
        _AlphaFadeAmount("Fade Amount", Range(-0.1, 1)) = -0.1 //55
        _AlphaFadeSmooth("Fade Transition", Range(0.0, 1.5)) = 0.075 //56
        _AlphaFadePow("Fade Power", Range(0.001, 10)) = 1 //57
    	
    	_TrailWidthPower("Trail Width Power", Range(0.1, 5.0)) = 1.0 //58
    	[AllIn1VfxGradient] _TrailWidthGradient("Trail Width Gradient", 2D) = "white" {} //59

        _GlowColor("Glow Color", Color) = (1,1,1,1) //60
        _Glow("Glow Color Intensity", float) = 0 //61
        _GlowGlobal("Global Glow Intensity", float) = 1 //62
        _GlowTex("Glow Mask Texture", 2D) = "white" {} //63

        _DepthGlowDist("Depth Distance", Range(0.01, 10.0)) = 0.5 //64
        _DepthGlowPow("Depth Power", Range(0.01, 10.0)) = 1 //65
        _DepthGlowColor("Glow Color", Color) = (1,1,1,1) //66
        _DepthGlow("Glow Color Intensity", float) = 1 //67
        _DepthGlowGlobal("Global Glow Intensity", float) = 1 //68
        
        _MaskTex("Mask Texture", 2D) = "white" {} //69
        _MaskPow("Mask Power", Range(0.001, 10)) = 1 //70
        
        _HsvShift("Hue Shift", Range(0, 360)) = 180 //71
		_HsvSaturation("Saturation", Range(0, 2)) = 1 //72
		_HsvBright("Brightness", Range(0, 2)) = 1 //73
        
        _RandomSh1Mult("Shape 1 Mult", Range(0, 1)) = 1.0 //74
        _RandomSh2Mult("Shape 2 Mult", Range(0, 1)) = 1.0 //75
        _RandomSh3Mult("Shape 3 Mult", Range(0, 1)) = 1.0 //76
        
        _PixelateSize("Pixelate size", Range(4, 512)) = 32 //77
        
        _DistortTex("Distortion Texture", 2D) = "black" {} //78
		_DistortAmount("Distortion Amount", Range(0, 10)) = 0.5 //79
		_DistortTexXSpeed("Scroll Speed X", Range(-50, 50)) = 5 //80
		_DistortTexYSpeed("Scroll Speed Y", Range(-50, 50)) = 5 //81
        
        [HDR] _BackFaceTint("Backface Tint", Color) = (0.5, 0.5, 0.5, 1) //82
    	[HDR] _FrontFaceTint("Frontface Tint", Color) = (1, 1, 1, 1) //83
        
        _ShakeUvSpeed("Shake Speed", Range(0, 50)) = 20 //84
		_ShakeUvX("X Multiplier", Range(-15, 15)) = 5 //85
		_ShakeUvY("Y Multiplier", Range(-15, 15)) = 4 //86
        
        _WaveAmount("Wave Amount", Range(0, 25)) = 7 //87
		_WaveSpeed("Wave Speed", Range(0, 25)) = 10 //88
		_WaveStrength("Wave Strength", Range(0, 25)) = 7.5 //89
		_WaveX("Wave X Axis", Range(0, 1)) = 0 //90
		_WaveY("Wave Y Axis", Range(0, 1)) = 0.5 //91
        
        _RoundWaveStrength("Wave Strength", Range(0, 1)) = 0.7 //92
		_RoundWaveSpeed("Wave Speed", Range(0, 5)) = 2 //93
        
        _TwistUvAmount("Twist Amount", Range(0, 3.1416)) = 1 //94
		_TwistUvPosX("Twist Pos X Axis", Range(0, 1)) = 0.5 //95
		_TwistUvPosY("Twist Pos Y Axis", Range(0, 1)) = 0.5 //96
		_TwistUvRadius("Twist Radius", Range(0, 3)) = 0.75 //97
        
        _HandDrawnAmount("Hand Drawn Amount", Range(0, 40)) = 10 //98
		_HandDrawnSpeed("Hand Drawn Speed", Range(1, 30)) = 5 //99
    	
    	_OffsetSh1("Shape 1 Offset Mult", Range(-5, 5)) = 1 //100
    	_OffsetSh2("Shape 2 Offset Mult", Range(-5, 5)) = 1 //101
    	_OffsetSh3("Shape 3 Offset Mult", Range(-5, 5)) = 1 //102
    	
    	_DistNormalMap("Normal Map", 2D) = "bump" {} //103
		_DistortionPower("Distortion Power", Float) = 10 //104
		_DistortionBlend("Distortion Blend", Range(0, 1)) = 1 //105
    	_DistortionScrollXSpeed("Scroll speed X Axis", Float) = 0 //106
		_DistortionScrollYSpeed("Scroll speed Y Axis", Float) = 0 //107
    	
    	_TextureScrollXSpeed("Speed X Axis", Float) = 1 //108
		_TextureScrollYSpeed("Speed Y Axis", Float) = 0 //109
    	
    	_VertOffsetTex("Offset Noise Texture", 2D) = "white" {} //110
		_VertOffsetAmount("Offset Amount", Range(0, 2)) = 0.5 //111
		_VertOffsetPower("Offset Power", Range(0.01, 10)) = 1 //112
		_VertOffsetTexXSpeed("Scroll Speed X", Range(-2, 2)) = 0.1 //113
		_VertOffsetTexYSpeed("Scroll Speed Y", Range(-2, 2)) = 0.1 //114
    	
    	_FadeTex("Fade Texture", 2D) = "white" {} //115
		_FadeAmount("Fade Amount", Range(-0.1, 1)) = -0.1 //116
		_FadeTransition("Fade Transition", Range(0.01, 0.75)) = 0.075 //117
		_FadePower("Fade Power", Range(0.001, 10)) = 1 //118
    	_FadeScrollXSpeed("Speed X Axis", Float) = 0 //119
		_FadeScrollYSpeed("Speed Y Axis", Float) = 0 //120
		_FadeBurnTex("Fade Burn Texture", 2D) = "white" {} //121
		[HDR] _FadeBurnColor("Fade Burn Color", Color) = (1,1,0,1) //122
		_FadeBurnWidth("Fade Burn Width", Range(0, 0.2)) = 0.01 //123
		_FadeBurnGlow("Fade Burn Glow", Range(1, 250)) = 5//124
    	
    	[HDR] _ColorGradingLight("Light Color Tint", Color) = (1,1,1,1) //125
    	[HDR] _ColorGradingMiddle("Mid Tone Color Tint", Color) = (1,1,1,1) //126
    	[HDR] _ColorGradingDark("Dark/Shadow Color Tint", Color) = (1,1,1,1) //127
    	_ColorGradingMidPoint("Mid Point", Range(0.01, 0.99)) = 0.5 //128
    	
    	_CamDistFadeStepMin("Far Fade Start Point", Range(0, 1000)) = 0.0 //129
        _CamDistFadeStepMax("Far Fade End Point", Range(0, 1000)) = 100 //130
        _CamDistProximityFade("Close Fade Start Point", Range(0, 250)) = 0.0 //131
    	
    	_ScreenUvShDistScale("Scale With Dist Amount", Range(0, 1)) = 1 //132
    	_ScreenUvSh2DistScale("Scale With Dist Amount", Range(0, 1)) = 1 //133
    	_ScreenUvSh3DistScale("Scale With Dist Amount", Range(0, 1)) = 1 //134
    	
    	[HDR] _RimColor("Rim Color", Color) = (1, 1, 1, 1) //135
        _RimBias("Rim Bias", Range(0, 1)) = 0 //136
        _RimScale("Rim Scale", Range(0, 25)) = 1 //137
        _RimPower("Rim Power", Range(0.1, 20.0)) = 5.0 //138
        _RimIntensity("Rim Intensity", Range(0.0, 50.0)) = 1  //139
        _RimAddAmount("Add Amount (0 is mult)", Range(0.0, 1.0)) = 1  //140
        _RimErodesAlpha("Erode Transparency", Range(0.0, 2.0)) = 0  //141
    	
        _Shape1MaskTex("Shape 1 Mask Texture", 2D) = "white" {} //142
        _Shape1MaskPow("Shape 1 Mask Power", Range(0.001, 10)) = 1 //143
    	
        _LightAmount("Light Amount", Range(0, 1)) = 0//144
        [HDR] _LightColor("Light Color", Color) = (1,1,1,1) //147
    	_ShadowAmount("Shadow Amount", Range(0, 1)) = 0.4//148
    	_ShadowStepMin("Shadow Min", Range(0, 1)) = 0.0 //149
        _ShadowStepMax("Shadow Max", Range(0, 1)) = 1.0 //148
        
        _PosterizeNumColors("Number of Colors", Range(1, 30)) = 5 //149
    	
    	_ShapeRotationOffset("Rotation Offset", Range(0, 6.28318530718)) = 0 //150
    	_ShapeRotationSpeed("Rotation Speed", Float) = 0 //151
    	_Shape2RotationOffset("Rotation Offset", Range(0, 6.28318530718)) = 0 //152
    	_Shape2RotationSpeed("Rotation Speed", Float) = 0 //153
    	_Shape3RotationOffset("Rotation Offset", Range(0, 6.28318530718)) = 0 //154
    	_Shape3RotationSpeed("Rotation Speed", Float) = 0 //155
    	
    	_Sh1BlendOffset("Shape 1 Blend Offset", Range(-5, 5)) = 0 //156
		_Sh2BlendOffset("Shape 2 Blend Offset", Range(-5, 5)) = 0 //157
		_Sh3BlendOffset("Shape 3 Blend Offset", Range(-5, 5)) = 0 //158
    	
        _DebugShape("Shape Debug Number", Int) = 1 //160 Needs to be last property
    }
	
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "CanUseSpriteAtlas" = "True" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane"
        }
        Blend [_SrcMode] [_DstMode]
        Cull [_CullingOption]
        ZWrite [_ZWrite]
        ZTest [_ZTestMode]
        ColorMask [_ColorMask]
        Lighting Off
        
        GrabPass{ } /////////////////Standard pipeline implementation

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #pragma shader_feature ADDITIVECONFIG_ON
            #pragma shader_feature PREMULTIPLYALPHA_ON
            #pragma shader_feature PREMULTIPLYCOLOR_ON
            #pragma shader_feature SPLITRGBA_ON
            #pragma shader_feature SHAPEADD_ON

            #pragma shader_feature FOG_ON /////////////////Pipeline specific implementation
            #pragma shader_feature SCREENDISTORTION_ON /////////////////Pipeline specific implementation
            #pragma shader_feature DISTORTUSECOL_ON /////////////////Pipeline specific implementation, inside SCREENDISTORTION_ON
            #pragma shader_feature DISTORTONLYBACK_ON /////////////////Pipeline specific implementation, inside SCREENDISTORTION_ON
            #pragma shader_feature SHAPE1SCREENUV_ON /////////////////Pipeline specific implementation
            #pragma shader_feature SHAPE2SCREENUV_ON /////////////////Pipeline specific implementation
            #pragma shader_feature SHAPE3SCREENUV_ON /////////////////Pipeline specific implementation

            #pragma shader_feature SHAPEDEBUG_ON
            
            #pragma shader_feature SHAPE1CONTRAST_ON
            #pragma shader_feature SHAPE1DISTORT_ON
            #pragma shader_feature SHAPE1ROTATE_ON
            #pragma shader_feature SHAPE1SHAPECOLOR_ON

            #pragma shader_feature SHAPE2_ON
            #pragma shader_feature SHAPE2CONTRAST_ON
            #pragma shader_feature SHAPE2DISTORT_ON
            #pragma shader_feature SHAPE2ROTATE_ON
            #pragma shader_feature SHAPE2SHAPECOLOR_ON

            #pragma shader_feature SHAPE3_ON
            #pragma shader_feature SHAPE3CONTRAST_ON
            #pragma shader_feature SHAPE3DISTORT_ON
            #pragma shader_feature SHAPE3ROTATE_ON
            #pragma shader_feature SHAPE3SHAPECOLOR_ON

            #pragma shader_feature GLOW_ON
            #pragma shader_feature GLOWTEX_ON
            #pragma shader_feature SOFTPART_ON /////////////////Pipeline specific implementation
            #pragma shader_feature DEPTHGLOW_ON /////////////////Pipeline specific implementation
            #pragma shader_feature MASK_ON
            #pragma shader_feature COLORRAMP_ON
            #pragma shader_feature COLORRAMPGRAD_ON
            #pragma shader_feature COLORGRADING_ON
            #pragma shader_feature HSV_ON
            #pragma shader_feature POSTERIZE_ON
            #pragma shader_feature PIXELATE_ON
            #pragma shader_feature DISTORT_ON
			#pragma shader_feature SHAKEUV_ON
            #pragma shader_feature WAVEUV_ON
			#pragma shader_feature ROUNDWAVEUV_ON
            #pragma shader_feature TWISTUV_ON
            #pragma shader_feature DOODLE_ON
            #pragma shader_feature OFFSETSTREAM_ON
            #pragma shader_feature TEXTURESCROLL_ON
            #pragma shader_feature VERTOFFSET_ON
            #pragma shader_feature RIM_ON /////////////////Pipeline specific implementation
            #pragma shader_feature BACKFACETINT_ON /////////////////Pipeline specific implementation
            #pragma shader_feature POLARUV_ON
            #pragma shader_feature POLARUVDISTORT_ON
            #pragma shader_feature SHAPE1MASK_ON
            #pragma shader_feature TRAILWIDTH_ON
            #pragma shader_feature LIGHTANDSHADOW_ON
            #pragma shader_feature SHAPETEXOFFSET_ON
            #pragma shader_feature SHAPEWEIGHTS_ON
            
            #pragma shader_feature ALPHACUTOFF_ON
            #pragma shader_feature ALPHASMOOTHSTEP_ON
            #pragma shader_feature FADE_ON
            #pragma shader_feature FADEBURN_ON
            #pragma shader_feature ALPHAFADE_ON
            #pragma shader_feature ALPHAFADEUSESHAPE1_ON
            #pragma shader_feature ALPHAFADEUSEREDCHANNEL_ON
            #pragma shader_feature ALPHAFADETRANSPARENCYTOO_ON
            #pragma shader_feature ALPHAFADEINPUTSTREAM_ON
            #pragma shader_feature CAMDISTFADE_ON

            #if FOG_ON
            #pragma multi_compile_fog
            #endif

            #include "UnityCG.cginc"
            #include "AllIn1VfxFunctions.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            	
                //z component is custom data from particle system and will be used as Timing Seed
            	//w is potentially used for alpha custom stream input
                float4 uv : TEXCOORD0;

            	#if OFFSETSTREAM_ON && !SHAPEWEIGHTS_ON
                half2 customData1 : TEXCOORD1; //x and y are the shapes uv offset
            	#elif SHAPEWEIGHTS_ON
            	half3 customData1 : TEXCOORD1; //z is the shapes weight offset
            	#endif

            	#if VERTOFFSET_ON || RIM_ON || BACKFACETINT_ON || LIGHTANDSHADOW_ON
                half3 normal : NORMAL;
            	#endif
            	
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                //z component is custom data from particle system and will be used as Timing Seed
            	//w is potentially used for alpha custom stream input
                float4 uvSeed : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half4 color : COLOR;

            	#if OFFSETSTREAM_ON && !SHAPEWEIGHTS_ON
            	half2 offsetCustomData : TEXCOORD1;
            	#elif SHAPEWEIGHTS_ON
            	half3 offsetCustomData : TEXCOORD1;
				#endif
            	
                #if SHAPE1DISTORT_ON && !POLARUVDISTORT_ON
                half2 uvSh1DistTex : TEXCOORD2;
                #endif
                #if SHAPE2DISTORT_ON && !POLARUVDISTORT_ON
                half2 uvSh2DistTex : TEXCOORD3;
                #endif
                #if SHAPE3DISTORT_ON && !POLARUVDISTORT_ON
                half2 uvSh3DistTex : TEXCOORD4;
                #endif

                #if SOFTPART_ON || DEPTHGLOW_ON || (SCREENDISTORTION_ON && DISTORTONLYBACK_ON)
                half4 projPos : TEXCOORD5;
                #endif

            	#if SCREENDISTORTION_ON || SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
            	half4 screenCoord : TEXCOORD6;
                #endif

            	#if DISTORT_ON && !POLARUVDISTORT_ON
				half2 uvDistTex : TEXCOORD7;
				#endif

            	#if CAMDISTFADE_ON || SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
				half4 worldPos : TEXCOORD8;
				#endif

            	#if RIM_ON || BACKFACETINT_ON || LIGHTANDSHADOW_ON
				half3 viewDir : TEXCOORD9;
            	half3 normal : NORMAL;
            	#endif

                #if FOG_ON
                UNITY_FOG_COORDS(10)
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
            	UNITY_VERTEX_OUTPUT_STEREO
            };

            #if SOFTPART_ON || DEPTHGLOW_ON || (SCREENDISTORTION_ON && DISTORTONLYBACK_ON)
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            #endif
            
            half4 _Color;
            half _Alpha;

            sampler2D _MainTex;
            half4 _MainTex_ST, _ShapeColor;
            half _ShapeXSpeed, _ShapeYSpeed, _ShapeColorWeight, _ShapeAlphaWeight;
            #if SHAPE1CONTRAST_ON
            half _ShapeContrast, _ShapeBrightness;
            #endif
            #if SHAPE1DISTORT_ON
            sampler2D _ShapeDistortTex;
            half4 _ShapeDistortTex_ST;
            half _ShapeDistortAmount, _ShapeDistortXSpeed, _ShapeDistortYSpeed;
            #endif
            #if SHAPE1ROTATE_ON
            half _ShapeRotationOffset, _ShapeRotationSpeed;
            #endif
            #if OFFSETSTREAM_ON
            half _OffsetSh1;
            #endif
            #if SHAPEWEIGHTS_ON
            half _Sh1BlendOffset;
            #endif

            #if SHAPE2_ON
            sampler2D _Shape2Tex;
            half4 _Shape2Tex_ST, _Shape2Color;
            half _Shape2XSpeed, _Shape2YSpeed, _Shape2ColorWeight, _Shape2AlphaWeight;
            #if SHAPE2CONTRAST_ON
            half _Shape2Contrast, _Shape2Brightness;
            #endif
            #if SHAPE2DISTORT_ON
            sampler2D _Shape2DistortTex;
            half4 _Shape2DistortTex_ST;
            half _Shape2DistortAmount, _Shape2DistortXSpeed, _Shape2DistortYSpeed;
            #endif
            #if SHAPE2ROTATE_ON
            half _Shape2RotationOffset, _Shape2RotationSpeed;
            #endif
            #if OFFSETSTREAM_ON
            half _OffsetSh2;
            #endif
            #if SHAPEWEIGHTS_ON
            half _Sh2BlendOffset;
            #endif
            #endif

            #if SHAPE3_ON
            sampler2D _Shape3Tex;
            half4 _Shape3Tex_ST, _Shape3Color;
            half _Shape3XSpeed, _Shape3YSpeed, _Shape3ColorWeight, _Shape3AlphaWeight;
            #if SHAPE3CONTRAST_ON
            half _Shape3Contrast, _Shape3Brightness;
            #endif
            #if SHAPE3DISTORT_ON
            sampler2D _Shape3DistortTex;
            half4 _Shape3DistortTex_ST;
            half _Shape3DistortAmount, _Shape3DistortXSpeed, _Shape3DistortYSpeed;
            #endif
            #if SHAPE3ROTATE_ON
            half _Shape3RotationOffset, _Shape3RotationSpeed;
            #endif
            #if OFFSETSTREAM_ON
            half _OffsetSh3;
            #endif
            #if SHAPEWEIGHTS_ON
            half _Sh3BlendOffset;
            #endif
            #endif

            #if GLOW_ON
            half4 _GlowColor;
            half _Glow, _GlowGlobal;
            #if GLOWTEX_ON
            sampler2D _GlowTex;
            half4 _GlowTex_ST;
            #endif
            #endif

            #if SOFTPART_ON
            half _SoftFactor;
            #endif

            #if DEPTHGLOW_ON
            half _DepthGlowDist, _DepthGlowPow, _DepthGlow, _DepthGlowGlobal;
            half4 _DepthGlowColor;
            #endif

            #if MASK_ON
            sampler2D _MaskTex;
            half4 _MaskTex_ST;
            half _MaskPow;
            #endif

            #if COLORRAMP_ON
            sampler2D _ColorRampTex;
            half _ColorRampLuminosity, _ColorRampBlend;
            #endif

            #if COLORRAMPGRAD_ON
            sampler2D _ColorRampTexGradient;
            #endif

            #if ALPHACUTOFF_ON
            half _AlphaCutoffValue;
            #endif

            #if ALPHASMOOTHSTEP_ON
            half _AlphaStepMin, _AlphaStepMax;
            #endif

            #if ALPHAFADE_ON
            half _AlphaFadeAmount, _AlphaFadeSmooth, _AlphaFadePow;
            #endif

            #if HSV_ON
			half _HsvShift, _HsvSaturation, _HsvBright;
			#endif

            #if POSTERIZE_ON
			half _PosterizeNumColors;
			#endif

            #if PIXELATE_ON
			half _PixelateSize;
			#endif

            #if DISTORT_ON
			sampler2D _DistortTex;
			half4 _DistortTex_ST;
			half _DistortTexXSpeed, _DistortTexYSpeed, _DistortAmount;
			#endif

            #if TEXTURESCROLL_ON
			half _TextureScrollXSpeed, _TextureScrollYSpeed;
			#endif

			#if SHAKEUV_ON
			half _ShakeUvSpeed, _ShakeUvX, _ShakeUvY;
			#endif

            #if WAVEUV_ON
			half _WaveAmount, _WaveSpeed, _WaveStrength, _WaveX, _WaveY;
			#endif

			#if ROUNDWAVEUV_ON
			half _RoundWaveStrength, _RoundWaveSpeed;
			#endif

            #if TWISTUV_ON
			half _TwistUvAmount, _TwistUvPosX, _TwistUvPosY, _TwistUvRadius;
			#endif

            #if DOODLE_ON
			half _HandDrawnAmount, _HandDrawnSpeed;
			#endif

            #if SCREENDISTORTION_ON
            sampler2D _DistNormalMap, _GrabTexture;
			half4 _DistNormalMap_ST, _GrabTexture_ST;
			half _DistortionPower, _DistortionBlend, _DistortionScrollXSpeed, _DistortionScrollYSpeed;
            #endif

            #if ROUNDWAVEUV_ON || SCREENDISTORTION_ON
            half4 _MainTex_TexelSize;
            #endif

            #if VERTOFFSET_ON
            sampler2D _VertOffsetTex;
			half4 _VertOffsetTex_ST;
			half _VertOffsetAmount, _VertOffsetPower, _VertOffsetTexXSpeed, _VertOffsetTexYSpeed;
            #endif

            #if FADE_ON
			sampler2D _FadeTex;
			half4 _FadeTex_ST;
			half _FadeAmount, _FadeTransition, _FadePower, _FadeScrollXSpeed, _FadeScrollYSpeed;
            #if FADEBURN_ON
			sampler2D _FadeBurnTex;
			half4 _FadeBurnColor, _FadeBurnTex_ST;
			half _FadeBurnWidth, _FadeBurnGlow;
			#endif
			#endif

            #if COLORGRADING_ON
            half3 _ColorGradingLight, _ColorGradingMiddle, _ColorGradingDark;
            half _ColorGradingMidPoint;
            #endif
            
			#if CAMDISTFADE_ON
            half _CamDistFadeStepMin, _CamDistFadeStepMax, _CamDistProximityFade;
            #endif

            #if SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
            half _ScreenUvShDistScale,_ScreenUvSh2DistScale, _ScreenUvSh3DistScale;
			#endif
            
            #if RIM_ON
            half _RimBias, _RimScale, _RimPower, _RimIntensity, _RimAddAmount, _RimErodesAlpha;
            half4 _RimColor;
            #endif

            #if BACKFACETINT_ON
			half4 _BackFaceTint, _FrontFaceTint;
            #endif

            #if SHAPEDEBUG_ON
			half _DebugShape;
            #endif

            #if SHAPE1MASK_ON
            sampler2D _Shape1MaskTex;
            half4 _Shape1MaskTex_ST;
            half _Shape1MaskPow;
            #endif

			#if TRAILWIDTH_ON
            half _TrailWidthPower;
			sampler2D _TrailWidthGradient;
            #endif

            #if LIGHTANDSHADOW_ON
            half3 _All1VfxLightDir;
            half _ShadowAmount, _ShadowStepMin, _ShadowStepMax, _LightAmount;
            half4 _LightColor;
            #endif

            #if SHAPETEXOFFSET_ON
            half _RandomSh1Mult, _RandomSh2Mult, _RandomSh3Mult;
            #endif
            
            UNITY_INSTANCING_BUFFER_START(Seeds)
            UNITY_DEFINE_INSTANCED_PROP(half, _TimingSeed)
            UNITY_INSTANCING_BUFFER_END(Seeds)

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
            	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvSeed = v.uv;
                o.color = v.color;

            	#if VERTOFFSET_ON
                const half time = v.uv.z + _Time.y;
            	half4 offsetUv = half4(TRANSFORM_TEX(v.uv.xy, _VertOffsetTex), 0, 0);
            	offsetUv.x += (time * _VertOffsetTexXSpeed) % 1;
				offsetUv.y += (time * _VertOffsetTexYSpeed) % 1;
                v.vertex.xyz += v.normal * _VertOffsetAmount * pow(tex2Dlod(_VertOffsetTex, offsetUv).r, _VertOffsetPower);
            	#endif
            	
                o.vertex = UnityObjectToClipPos(v.vertex);

            	#if CAMDISTFADE_ON || SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				#endif

            	#if OFFSETSTREAM_ON
				o.offsetCustomData.x = v.customData1.x;
            	o.offsetCustomData.y = v.customData1.y;
				#endif

            	#if SHAPEWEIGHTS_ON
				o.offsetCustomData.z = v.customData1.z;
				#endif

                #if SHAPE1DISTORT_ON && !POLARUVDISTORT_ON
                o.uvSh1DistTex = TRANSFORM_TEX(v.uv.xy, _ShapeDistortTex);
                #endif

                #if SHAPE2_ON
                #if SHAPE2DISTORT_ON && !POLARUVDISTORT_ON
                o.uvSh2DistTex = TRANSFORM_TEX(v.uv.xy, _Shape2DistortTex);
                #endif
                #endif

                #if SHAPE3_ON
                #if SHAPE3DISTORT_ON && !POLARUVDISTORT_ON
                o.uvSh3DistTex = TRANSFORM_TEX(v.uv.xy, _Shape3DistortTex);
                #endif
                #endif

            	#if SCREENDISTORTION_ON || SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
				o.screenCoord = ComputeGrabScreenPos(o.vertex);
				#endif

            	#if DISTORT_ON && !POLARUVDISTORT_ON
				o.uvDistTex = TRANSFORM_TEX(v.uv, _DistortTex);
				#endif

                #if FOG_ON
                UNITY_TRANSFER_FOG(o, o.vertex); //Standard pipeline only
                #endif

                #if SOFTPART_ON || DEPTHGLOW_ON || (SCREENDISTORTION_ON && DISTORTONLYBACK_ON)
                o.projPos = ComputeScreenPos(o.vertex); //Standard pipeline only
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif

            	#if RIM_ON || BACKFACETINT_ON || LIGHTANDSHADOW_ON
				o.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex)));
            	o.normal = UnityObjectToWorldNormal(v.normal);
            	#endif

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
            	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                float seed = i.uvSeed.z + UNITY_ACCESS_INSTANCED_PROP(Seeds, _TimingSeed);
                float time = _Time.y + seed;

            	#if SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
				half2 originalUvs = i.uvSeed.xy;
            	#endif

            	#if PIXELATE_ON
				i.uvSeed.xy = floor(i.uvSeed.xy * _PixelateSize) / _PixelateSize;
				#endif

            	#if TWISTUV_ON
				half2 tempUv = i.uvSeed.xy - half2(_TwistUvPosX * _MainTex_ST.x, _TwistUvPosY * _MainTex_ST.y);
				_TwistUvRadius *= (_MainTex_ST.x + _MainTex_ST.y) / 2;
				half percent = (_TwistUvRadius - length(tempUv)) / _TwistUvRadius;
				half theta = percent * percent * (2.0 * sin(_TwistUvAmount)) * 8.0;
				half s = sin(theta);
				half c = cos(theta);
				half beta = max(sign(_TwistUvRadius - length(tempUv)), 0.0);
				tempUv = half2(dot(tempUv, half2(c, -s)), dot(tempUv, half2(s, c))) * beta +	tempUv * (1 - beta);
				tempUv += half2(_TwistUvPosX * _MainTex_ST.x, _TwistUvPosY * _MainTex_ST.y);
				i.uvSeed.xy = tempUv;
				#endif

				#if DOODLE_ON
				half2 uvCopy = i.uvSeed.xy;
				_HandDrawnSpeed = (floor((_Time + seed) * 20 * _HandDrawnSpeed) / _HandDrawnSpeed) * _HandDrawnSpeed;
				uvCopy.x = sin((uvCopy.x * _HandDrawnAmount + _HandDrawnSpeed) * 4);
				uvCopy.y = cos((uvCopy.y * _HandDrawnAmount + _HandDrawnSpeed) * 4);
				i.uvSeed.xy = lerp(i.uvSeed.xy, i.uvSeed.xy + uvCopy, 0.0005 * _HandDrawnAmount);
				#endif

				#if SHAKEUV_ON
				half xShake = sin((_Time + seed) * _ShakeUvSpeed * 50) * _ShakeUvX;
				half yShake = cos((_Time + seed) * _ShakeUvSpeed * 50) * _ShakeUvY;
				i.uvSeed.xy += half2(xShake * 0.012, yShake * 0.01);
				#endif

            	#if WAVEUV_ON
				half2 uvWave = half2(_WaveX * _MainTex_ST.x, _WaveY * _MainTex_ST.y) - i.uvSeed.xy;
				#if ATLAS_ON
				uvWave = half2(_WaveX, _WaveY) - uvRect;
				#endif
				uvWave.x *= _ScreenParams.x / _ScreenParams.y;
				half angWave = (sqrt(dot(uvWave, uvWave)) * _WaveAmount) - ((time *  _WaveSpeed) % 360.0);
				i.uvSeed.xy = i.uvSeed.xy + normalize(uvWave) * sin(angWave) * (_WaveStrength / 1000.0);
				#endif

				#if ROUNDWAVEUV_ON
				half xWave = ((0.5 * _MainTex_ST.x) - i.uvSeed.x);
				half yWave = ((0.5 * _MainTex_ST.y) - i.uvSeed.y) * (_MainTex_TexelSize.w / _MainTex_TexelSize.z);
				half ripple = -sqrt(xWave*xWave + yWave* yWave);
            	i.uvSeed.xy += (sin((ripple + time * (_RoundWaveSpeed/10.0)) / 0.015) * (_RoundWaveStrength/10.0)) % 1;
				#endif

            	#if POLARUV_ON
            	half2 prePolarUvs = i.uvSeed.xy;
            	i.uvSeed.xy = i.uvSeed.xy - half2(0.5, 0.5);
				i.uvSeed.xy = half2(atan2(i.uvSeed.y, i.uvSeed.x) / (1.0 * 6.28318530718), length(i.uvSeed.xy) * 2.0);
            	i.uvSeed.xy *= _MainTex_ST.xy;
				#endif

				#if DISTORT_ON
            	#if POLARUVDISTORT_ON
            	half2 distortUvs = TRANSFORM_TEX(i.uvSeed.xy, _DistortTex);
            	#else
            	half2 distortUvs = i.uvDistTex.xy;
            	#endif
				distortUvs.x += ((_Time + seed) * _DistortTexXSpeed) % 1;
				distortUvs.y += ((_Time + seed) * _DistortTexYSpeed) % 1;
				#if ATLAS_ON
				i.uvDistTex = half2((i.uvDistTex.x - _MinXUV) / (_MaxXUV - _MinXUV), (i.uvDistTex.y - _MinYUV) / (_MaxYUV - _MinYUV));
				#endif
				half distortAmnt = (tex2D(_DistortTex, distortUvs).r - 0.5) * 0.2 * _DistortAmount;
				i.uvSeed.x += distortAmnt;
				i.uvSeed.y += distortAmnt;
				#endif

            	#if TEXTURESCROLL_ON
				i.uvSeed.x += (time * _TextureScrollXSpeed) % 1;
				i.uvSeed.y += (time * _TextureScrollYSpeed) % 1;
				#endif

            	#if TRAILWIDTH_ON
            	half width = pow(tex2D(_TrailWidthGradient, i.uvSeed).r, _TrailWidthPower);
            	i.uvSeed.y = (i.uvSeed.y * 2 - 1) / width * 0.5 + 0.5;
            	clip(i.uvSeed.y);
            	clip(1 - i.uvSeed.y);
            	#endif

				float2 shape1Uv = i.uvSeed.xy;
            	#if SHAPE2_ON
            	float2 shape2Uv = shape1Uv;
            	#endif
            	#if SHAPE3_ON
            	float2 shape3Uv = shape1Uv;
            	#endif

            	#if CAMDISTFADE_ON || SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
            	half camDistance = distance(i.worldPos, _WorldSpaceCameraPos);
				#endif
            	
            	#if SHAPE1SCREENUV_ON || SHAPE2SCREENUV_ON || SHAPE3SCREENUV_ON
            	half2 uvOffsetPostFx = i.uvSeed.xy - originalUvs;
				i.uvSeed.xy = i.screenCoord.xy / i.screenCoord.w;
				i.uvSeed.x = i.uvSeed.x * (_ScreenParams.x / _ScreenParams.y);
				i.uvSeed.x -= 0.5;
            	i.uvSeed.xy -= uvOffsetPostFx;
            	originalUvs += uvOffsetPostFx;
            	half distanceZoom = camDistance * 0.1;
            	half2 scaleWithDistUvs = i.uvSeed.xy * distanceZoom + ((-distanceZoom * 0.5) + 0.5);
            	#if SHAPE1SCREENUV_ON
            	shape1Uv = lerp(i.uvSeed.xy, scaleWithDistUvs, _ScreenUvShDistScale);
            	#else
            	shape1Uv = originalUvs;
            	#endif
            	#if SHAPE2SCREENUV_ON && SHAPE2_ON
            	shape2Uv = lerp(i.uvSeed.xy, scaleWithDistUvs, _ScreenUvSh2DistScale);
            	#else
            	#if SHAPE2_ON
            	shape2Uv = originalUvs;
            	#endif
            	#endif
            	#if SHAPE3SCREENUV_ON && SHAPE3_ON
            	shape3Uv = lerp(i.uvSeed.xy, scaleWithDistUvs, _ScreenUvSh3DistScale);
            	#else
            	#if SHAPE3_ON
            	shape3Uv = originalUvs;
            	#endif
            	#endif
				#endif

                shape1Uv = TRANSFORM_TEX(shape1Uv, _MainTex);
            	#if OFFSETSTREAM_ON
				shape1Uv.x += i.offsetCustomData.x * _OffsetSh1;
                shape1Uv.y += i.offsetCustomData.y * _OffsetSh1;
				#endif
            	#if SHAPETEXOFFSET_ON
				shape1Uv += seed * _RandomSh1Mult;
				#endif
                #if SHAPE1DISTORT_ON
            	#if POLARUVDISTORT_ON
            	half2 sh1DistortUvs = TRANSFORM_TEX(i.uvSeed.xy, _ShapeDistortTex);
            	#else
            	half2 sh1DistortUvs = i.uvSh1DistTex.xy;
            	#endif
                sh1DistortUvs.x += ((time + seed) * _ShapeDistortXSpeed) % 1;
                sh1DistortUvs.y += ((time + seed) * _ShapeDistortYSpeed) % 1;
                half distortAmount = (tex2D(_ShapeDistortTex, sh1DistortUvs).r - 0.5) * 0.2 * _ShapeDistortAmount;
                shape1Uv.x += distortAmount;
                shape1Uv.y += distortAmount;
                #endif
            	#if SHAPE1ROTATE_ON
            	shape1Uv = RotateUvs(shape1Uv, _ShapeRotationOffset + ((_ShapeRotationSpeed * time) % 6.28318530718), _MainTex_ST);
            	#endif
            	half4 shape1 = SampleTextureWithScroll(_MainTex, shape1Uv, _ShapeXSpeed, _ShapeYSpeed, time);
                #if SHAPE1SHAPECOLOR_ON
                shape1.a = shape1.r;
                shape1.rgb = _ShapeColor.rgb;
                #else
                shape1 *= _ShapeColor;
                #endif
                #if SHAPE1CONTRAST_ON
                #if SHAPE1SHAPECOLOR_ON
                shape1.a = saturate((shape1.a - 0.5) * _ShapeContrast + 0.5 + _ShapeBrightness);
                #else
                shape1.rgb = max(0, (shape1.rgb - half3(0.5, 0.5, 0.5)) * _ShapeContrast + half3(0.5, 0.5, 0.5) + _ShapeBrightness);
                #endif
                #endif

                #if SHAPE2_ON
            	shape2Uv = TRANSFORM_TEX(shape2Uv, _Shape2Tex);
            	#if OFFSETSTREAM_ON
				shape2Uv.x += i.offsetCustomData.x * _OffsetSh2;
                shape2Uv.y += i.offsetCustomData.y * _OffsetSh2;
				#endif
            	#if SHAPETEXOFFSET_ON
				shape2Uv += seed * _RandomSh2Mult;
				#endif
                #if SHAPE2DISTORT_ON
            	#if POLARUVDISTORT_ON
            	half2 sh2DistortUvs = TRANSFORM_TEX(i.uvSeed.xy, _Shape2DistortTex);
            	#else
            	half2 sh2DistortUvs = i.uvSh2DistTex.xy;
            	#endif
                sh2DistortUvs.x += ((time + seed) * _Shape2DistortXSpeed) % 1;
                sh2DistortUvs.y += ((time + seed) * _Shape2DistortYSpeed) % 1;
                half distortAmnt2 = (tex2D(_Shape2DistortTex, sh2DistortUvs).r - 0.5) * 0.2 * _Shape2DistortAmount;
                shape2Uv.x += distortAmnt2;
                shape2Uv.y += distortAmnt2;
                #endif
            	#if SHAPE2ROTATE_ON
            	shape2Uv = RotateUvs(shape2Uv, _Shape2RotationOffset + ((_Shape2RotationSpeed * time) % 6.28318530718), _Shape2Tex_ST);
            	#endif
                half4 shape2 = SampleTextureWithScroll(_Shape2Tex, shape2Uv, _Shape2XSpeed, _Shape2YSpeed, time);
                #if SHAPE2SHAPECOLOR_ON
                shape2.a = shape2.r;
                shape2.rgb = _Shape2Color.rgb;
                #else
                shape2 *= _Shape2Color;
                #endif
                #if SHAPE2CONTRAST_ON
                #if SHAPE2SHAPECOLOR_ON
                shape2.a = max(0, (shape2.a - 0.5) * _Shape2Contrast + 0.5 + _Shape2Brightness);
                #else
                shape2.rgb = max(0, (shape2.rgb - half3(0.5, 0.5, 0.5)) * _Shape2Contrast + half3(0.5, 0.5, 0.5) + _Shape2Brightness);
                #endif
                #endif
                #endif

                #if SHAPE3_ON
                shape3Uv = TRANSFORM_TEX(shape3Uv, _Shape3Tex);
            	#if OFFSETSTREAM_ON
				shape3Uv.x += i.offsetCustomData.x * _OffsetSh3;
                shape3Uv.y += i.offsetCustomData.y * _OffsetSh3;
				#endif
            	#if SHAPETEXOFFSET_ON
				shape3Uv += seed * _RandomSh3Mult;
				#endif
                #if SHAPE3DISTORT_ON
            	#if POLARUVDISTORT_ON
            	half2 sh3DistortUvs = TRANSFORM_TEX(i.uvSeed.xy, _Shape3DistortTex);
            	#else
            	half2 sh3DistortUvs = i.uvSh3DistTex.xy;
            	#endif
                sh3DistortUvs.x += ((time + seed) * _Shape3DistortXSpeed) % 1;
                sh3DistortUvs.y += ((time + seed) * _Shape3DistortYSpeed) % 1;
                half distortAmnt3 = (tex2D(_Shape3DistortTex, sh3DistortUvs).r - 0.5) * 0.3 * _Shape3DistortAmount;
                shape3Uv.x += distortAmnt3;
                shape3Uv.y += distortAmnt3;
                #endif
            	#if SHAPE3ROTATE_ON
            	shape3Uv = RotateUvs(shape3Uv, _Shape3RotationOffset + ((_Shape3RotationSpeed * time) % 6.28318530718), _Shape3Tex_ST);
            	#endif
                half4 shape3 = SampleTextureWithScroll(_Shape3Tex, shape3Uv, _Shape3XSpeed, _Shape3YSpeed, time);
                #if SHAPE3SHAPECOLOR_ON
                shape3.a = shape3.r;
                shape3.rgb = _Shape3Color.rgb;
                #else
                shape3 *= _Shape3Color;
                #endif
                #if SHAPE3CONTRAST_ON
                #if SHAPE3SHAPECOLOR_ON
                shape3.a = max(0, (shape3.a - 0.5) * _Shape3Contrast + 0.5 + _Shape3Brightness);
                #else
                shape3.rgb = max(0, (shape3.rgb - half3(0.5, 0.5, 0.5)) * _Shape3Contrast + half3(0.5, 0.5, 0.5) + _Shape3Brightness);
                #endif
                #endif
                #endif

            	//ALWAYS UNCOMMENT THE FOLLOWING CODE BLOCK-------------------------------------------
            	#if SHAPEDEBUG_ON
            	if(_DebugShape < 1.5) return shape1;
            	#if SHAPE2_ON
            	else if (_DebugShape < 2.5) return shape2;
            	#endif
            	#if SHAPE3_ON
            	else return shape3;
            	#endif
                #endif

                half4 col = shape1;
                //Mix all shapes pre: change weights if custom vertex effect active
            	#if SHAPEWEIGHTS_ON
            	half shapeWeightOffset;
				#if SHAPE2_ON
            	shapeWeightOffset = i.offsetCustomData.z * _Sh1BlendOffset;
            	_ShapeColorWeight = max(0, _ShapeColorWeight + shapeWeightOffset);
            	_ShapeAlphaWeight = max(0, _ShapeAlphaWeight + shapeWeightOffset);
				shapeWeightOffset = i.offsetCustomData.z * _Sh2BlendOffset;
            	_Shape2ColorWeight = max(0, _Shape2ColorWeight + shapeWeightOffset);
            	_Shape2AlphaWeight = max(0, _Shape2AlphaWeight + shapeWeightOffset);
				#endif
            	#if SHAPE3_ON
				shapeWeightOffset = i.offsetCustomData.z * _Sh3BlendOffset;
            	_Shape3ColorWeight = max(0, _Shape3ColorWeight + shapeWeightOffset);
            	_Shape3AlphaWeight = max(0, _Shape3AlphaWeight + shapeWeightOffset);
				#endif
				#endif
            	//Mix all shapes
                #if SHAPE2_ON
                #if !SPLITRGBA_ON
				_ShapeAlphaWeight = _ShapeColorWeight;
				_Shape2AlphaWeight = _Shape2ColorWeight;
                #endif
                #if SHAPE3_ON //Shape3 On
                #if !SPLITRGBA_ON
                _Shape3AlphaWeight = _Shape3ColorWeight;
                #endif
            	#if SHAPEADD_ON
				col.rgb = ((shape1.rgb * _ShapeColorWeight) + (shape2.rgb * _Shape2ColorWeight)) + (shape3.rgb * _Shape3ColorWeight);
                col.a = saturate(max(shape3.a * _Shape3AlphaWeight, max(shape1.a * _ShapeAlphaWeight, shape2.a * _Shape2AlphaWeight)));
            	#else
            	col.rgb = ((shape1.rgb * _ShapeColorWeight) * (shape2.rgb * _Shape2ColorWeight)) * (shape3.rgb * _Shape3ColorWeight);
                col.a = saturate(((shape1.a * _ShapeAlphaWeight) * (shape2.a * _Shape2AlphaWeight)) * (shape3.a * _Shape3AlphaWeight));
            	#endif
                #else //Shape3 Off
            	#if SHAPEADD_ON
				col.rgb = (shape1.rgb * _ShapeColorWeight) + (shape2.rgb * _Shape2ColorWeight);
			    col.a = saturate(max(shape1.a * _ShapeAlphaWeight, shape2.a * _Shape2AlphaWeight));
            	#else
            	col.rgb = (shape1.rgb * _ShapeColorWeight) * (shape2.rgb * _Shape2ColorWeight);
			    col.a = saturate((shape1.a * _ShapeAlphaWeight) * (shape2.a * _Shape2AlphaWeight));
            	#endif
                #endif
                #endif

            	#if SHAPE1MASK_ON
            	col = lerp(col, shape1, pow(tex2D(_Shape1MaskTex, TRANSFORM_TEX(i.uvSeed.xy, _Shape1MaskTex)).r, _Shape1MaskPow));
            	#endif

            	#if PREMULTIPLYCOLOR_ON
            	half luminance = 0;
            	luminance = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;
                col.a = min(luminance, col.a);
                #endif

                col.rgb *= _Color * i.color.rgb;
                #if PREMULTIPLYALPHA_ON
                col.rgb *= col.a;
                #endif
                //col.rgb = saturate(col.rgb); //Removed to allow for HDR shape color. Contrast and shape combinations can now overexpose the result

            	#if !PREMULTIPLYCOLOR_ON && (COLORRAMP_ON || ALPHAFADE_ON || COLORGRADING_ON || FADE_ON || (ADDITIVECONFIG_ON && (GLOW_ON || DEPTHGLOW_ON)))
                half luminance = 0;
            	luminance = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;
            	#endif

            	#if (FADE_ON || ALPHAFADE_ON) && ALPHAFADEINPUTSTREAM_ON
            	col.a *= i.color.a;
				i.color.a = i.uvSeed.w;
            	#endif
            	
            	#if FADE_ON
            	half preFadeAlpha = col.a;
            	_FadeAmount = saturate(_FadeAmount + (1 - i.color.a));
            	_FadeTransition = max(0.01, _FadeTransition * EaseOutQuint(saturate(_FadeAmount)));
            	half2 fadeUv;
            	fadeUv = i.uvSeed.xy + seed;
            	fadeUv.x += (time * _FadeScrollXSpeed) % 1;
                fadeUv.y += (time * _FadeScrollYSpeed) % 1;
            	half2 tiledUvFade1 = TRANSFORM_TEX(fadeUv, _FadeTex);
            	#if ADDITIVECONFIG_ON && !PREMULTIPLYCOLOR_ON
                preFadeAlpha *= luminance;
            	#endif
            	_FadeAmount = saturate(pow(_FadeAmount, _FadePower));
            	#if FADEBURN_ON
				half2 tiledUvFade2 = TRANSFORM_TEX(fadeUv, _FadeBurnTex);
				half fadeSample = tex2D(_FadeTex, tiledUvFade1).r;
				half fadeNaturalEdge = saturate(smoothstep(0.0 , _FadeTransition, RemapFloat(1.0 - _FadeAmount, 0.0, 1.0, -1.0, 1.0) + fadeSample));
            	col.a *= fadeNaturalEdge;
            	half fadeBurn = saturate(smoothstep(0.0 , _FadeTransition + _FadeBurnWidth, RemapFloat(1.0 - _FadeAmount, 0.0, 1.0, -1.0, 1.0) + fadeSample));
            	fadeBurn = fadeNaturalEdge - fadeBurn;
				_FadeBurnColor.rgb *= _FadeBurnGlow;
				col.rgb += fadeBurn * tex2D(_FadeBurnTex, tiledUvFade2).rgb * _FadeBurnColor.rgb * preFadeAlpha;
            	#else
				half fadeSample = tex2D(_FadeTex, tiledUvFade1).r;
				float fade = saturate(smoothstep(0.0 , _FadeTransition, RemapFloat(1.0 - _FadeAmount, 0.0, 1.0, -1.0, 1.0) + fadeSample));
				col.a *= fade;
            	#endif
            	#if ALPHAFADETRANSPARENCYTOO_ON
                col.a *= 1 - _FadeAmount;
                #endif
				#endif

            	#if ALPHAFADE_ON
            	half alphaFadeLuminance;
            	_AlphaFadeAmount = saturate(_AlphaFadeAmount + (1 - i.color.a));
            	_AlphaFadeAmount = saturate(pow(_AlphaFadeAmount, _AlphaFadePow));
            	_AlphaFadeSmooth = max(0.01, _AlphaFadeSmooth * EaseOutQuint(saturate(_AlphaFadeAmount)));
                #if ALPHAFADEUSESHAPE1_ON
                alphaFadeLuminance = shape1.r;
                #else
                alphaFadeLuminance = luminance;
                #endif
                alphaFadeLuminance = saturate(alphaFadeLuminance - 0.001);
            	#if ALPHAFADEUSEREDCHANNEL_ON
                col.a *= col.r;
                #endif
                col.a = saturate(col.a);
				float alphaFade = saturate(smoothstep(0.0 , _AlphaFadeSmooth, RemapFloat(1.0 - _AlphaFadeAmount, 0.0, 1.0, -1.0, 1.0) + alphaFadeLuminance));
				col.a *= alphaFade;
            	#if ALPHAFADETRANSPARENCYTOO_ON
                col.a *= 1 - _AlphaFadeAmount;
            	#endif
                #endif

            	#if BACKFACETINT_ON
            	col.rgb = lerp(col.rgb * _BackFaceTint, col.rgb * _FrontFaceTint, step(0, dot(i.normal, i.viewDir)));
            	#endif

				#if LIGHTANDSHADOW_ON
                half NdL = saturate(dot(i.normal, -_All1VfxLightDir));
                col.rgb += _LightColor * _LightAmount * NdL;
                NdL = max(_ShadowAmount, NdL);
            	NdL = smoothstep(_ShadowStepMin, _ShadowStepMax, NdL);
            	col.rgb *= NdL;
				#endif

            	#if COLORGRADING_ON
				col.rgb *= lerp(lerp(_ColorGradingDark, _ColorGradingMiddle, luminance/_ColorGradingMidPoint),
					lerp(_ColorGradingMiddle, _ColorGradingLight, (luminance - _ColorGradingMidPoint)/(1.0 - _ColorGradingMidPoint)), step(_ColorGradingMidPoint, luminance));
            	#endif

                #if COLORRAMP_ON
                half colorRampLuminance = saturate(luminance + _ColorRampLuminosity);
                #if COLORRAMPGRAD_ON
                half4 colorRampRes = tex2D(_ColorRampTexGradient, half2(colorRampLuminance, 0));
                #else
            	half4 colorRampRes = tex2D(_ColorRampTex, half2(colorRampLuminance, 0));
                #endif
            	col.rgb = lerp(col.rgb, colorRampRes.rgb, _ColorRampBlend);
                col.a = lerp(col.a, saturate(col.a * colorRampRes.a), _ColorRampBlend);
                #endif

            	#if POSTERIZE_ON && !POSTERIZEOUTLINE_ON
            	col.rgb = floor(col.rgb / (1.0 / _PosterizeNumColors)) * (1.0 / _PosterizeNumColors);
				#endif
                
                #if SOFTPART_ON || DEPTHGLOW_ON
                half sceneDepthDiff = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))) - i.projPos.z;
                #endif

                #if SOFTPART_ON
                half sceneZMult = saturate(_SoftFactor * sceneDepthDiff);
                col.a *= sceneZMult;
                #endif

            	#if RIM_ON
				half NdV = 1 - abs(dot(i.normal, i.viewDir));
            	half rimFactor = saturate(_RimBias + _RimScale * pow(NdV, _RimPower));
                half4 rimCol = _RimColor * rimFactor;
                rimCol.rgb *= _RimIntensity;
            	col.rgb = lerp(col.rgb * (rimCol.rgb + half3(1,1,1)), col.rgb + rimCol.rgb, _RimAddAmount);
            	col.a = saturate(col.a * (1 - rimFactor * _RimErodesAlpha));
            	#endif
   
                #if DEPTHGLOW_ON
                half depthGlowMask = saturate(_DepthGlowDist * pow((1 - sceneDepthDiff), _DepthGlowPow));
                col.rgb = lerp(col.rgb, _DepthGlowGlobal * col.rgb, depthGlowMask);
            	half depthGlowMult = 1;
            	#if ADDITIVECONFIG_ON
            	depthGlowMult = luminance;
            	#endif
                col.rgb += _DepthGlowColor.rgb * _DepthGlow * depthGlowMask * col.a * depthGlowMult;
                #endif

                #if GLOW_ON
                half glowMask = 1;
                #if GLOWTEX_ON
                glowMask = tex2D(_GlowTex, TRANSFORM_TEX(i.uvSeed.xy, _GlowTex));
                #endif
                col.rgb *= _GlowGlobal * glowMask;
				half glowMult = 1;
            	#if ADDITIVECONFIG_ON
            	glowMult = luminance;
            	#endif
                col.rgb += _GlowColor.rgb * _Glow * glowMask * col.a * glowMult;
                #endif

            	#if HSV_ON
				half3 resultHsv = half3(col.rgb);
				half cosHsv = _HsvBright * _HsvSaturation * cos(_HsvShift * 3.14159265 / 180);
				half sinHsv = _HsvBright * _HsvSaturation * sin(_HsvShift * 3.14159265 / 180);
				resultHsv.x = (.299 * _HsvBright + .701 * cosHsv + .168 * sinHsv) * col.x
					+ (.587 * _HsvBright - .587 * cosHsv + .330 * sinHsv) * col.y
					+ (.114 * _HsvBright - .114 * cosHsv - .497 * sinHsv) * col.z;
				resultHsv.y = (.299 * _HsvBright - .299 * cosHsv - .328 * sinHsv) *col.x
					+ (.587 * _HsvBright + .413 * cosHsv + .035 * sinHsv) * col.y
					+ (.114 * _HsvBright - .114 * cosHsv + .292 * sinHsv) * col.z;
				resultHsv.z = (.299 * _HsvBright - .3 * cosHsv + 1.25 * sinHsv) * col.x
					+ (.587 * _HsvBright - .588 * cosHsv - 1.05 * sinHsv) * col.y
					+ (.114 * _HsvBright + .886 * cosHsv - .203 * sinHsv) * col.z;
				col.rgb = resultHsv;
				#endif

            	#if CAMDISTFADE_ON
            	col.a *= 1 - saturate(smoothstep(_CamDistFadeStepMin, _CamDistFadeStepMax, camDistance));
            	col.a *= smoothstep(0.0, _CamDistProximityFade, camDistance);
				#endif

                #if MASK_ON
            	half2 maskUv = i.uvSeed.xy;
            	#if POLARUV_ON
				maskUv = prePolarUvs;
				#endif
            	half4 maskSample = tex2D(_MaskTex, TRANSFORM_TEX(maskUv, _MaskTex));
                half mask = pow(min(maskSample.r, maskSample.a), _MaskPow);
                col.a *= mask;
                #endif

            	#if ALPHASMOOTHSTEP_ON
                col.a = smoothstep(_AlphaStepMin, _AlphaStepMax, col.a);
                #endif
                
                #if ALPHACUTOFF_ON
                clip((1 - _AlphaCutoffValue) - (1 - col.a) - 0.01);
                #endif

            	#if FOG_ON
                UNITY_APPLY_FOG(i.fogCoord, col);
                #endif

            	//Don't use a starting i.color.a lower than 1 unless using vertex stream dissolve when using a FADE effect
            	#if !FADE_ON && !ALPHAFADE_ON
                col.a *= _Alpha * i.color.a;
            	#endif
            	#if FADE_ON || ALPHAFADE_ON
                col.a *= _Alpha;
            	#endif
                #if ADDITIVECONFIG_ON
                col.rgb *= col.a;
                #endif

            	#if SCREENDISTORTION_ON
            	#if DISTORTUSECOL_ON
            	half4 normalMap = col;
            	normalMap.b = 1;
            	normalMap *= col.r;
            	#else
            	half2 distortionUv = TRANSFORM_TEX(i.uvSeed.xy, _DistNormalMap);
            	distortionUv.x += (time * _DistortionScrollXSpeed) % 1;
                distortionUv.y += (time * _DistortionScrollYSpeed) % 1;
            	half4 normalMap = tex2D(_DistNormalMap, distortionUv);
            	#endif
            	#if DISTORTONLYBACK_ON
				half4 originalScreenCoord = i.screenCoord;
            	#endif
            	half3 usableNormals = UnpackNormal(normalMap);
            	half2 distortUvOffset = usableNormals.rg * _DistortionPower * i.color.a * i.screenCoord.z * _GrabTexture_ST.xy * normalMap.a;
				i.screenCoord.xy = distortUvOffset + i.screenCoord.xy;
            	half3 distortCol = tex2Dproj(_GrabTexture, i.screenCoord).rgb;
            	#if DISTORTONLYBACK_ON
				i.projPos.xy += distortUvOffset;
				half frontMask = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))) - i.projPos.z;
            	frontMask = 1 - saturate(frontMask);
            	col.rgb = lerp(lerp(col.rgb, saturate(distortCol.rgb), _DistortionBlend), lerp(col.rgb, tex2Dproj(_GrabTexture, originalScreenCoord).rgb, _DistortionBlend), frontMask);
            	#else
            	col.rgb = lerp(col.rgb, saturate(distortCol.rgb), _DistortionBlend);
            	#endif
				#endif
            	
                return col;
            }
            ENDCG
        }
    }
    CustomEditor "AllIn1VfxCustomMaterialEditor"
	Fallback "Hidden/InternalErrorShader"
}