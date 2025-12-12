Shader "Dancing Line Fanmade/Standard/Transparent"
{
    Properties 
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Emission ("Emission Color", Color) = (0,0,0,0)
        _EmissionTex ("Emission Texture", 2D) = "white"{}
    }
    SubShader 
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200
        
        BlendOp Add
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back
        
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade fragment:surf

        sampler2D _MainTex;
        sampler2D _EmissionTex;
        fixed4 _Color;
        fixed4 _Emission;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Emission = tex2D (_EmissionTex, IN.uv_EmissionTex) * _Emission;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}