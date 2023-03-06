Shader "AllIn1Vfx/Others/ZWrite"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderQueue"="Geometry+1"}

        Pass
        {
            Zwrite On
            Offset 0, 1
            ColorMask Off
        }
    }
}