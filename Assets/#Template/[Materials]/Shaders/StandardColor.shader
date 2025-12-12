Shader "Dancing Line Fanmade/Standard/Color"
{
    Properties 
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Emission ("Emission Color", Color) = (0,0,0,0)
        _EmissionTex("Emission Texture", 2D) = "white"{}
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert

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
        }
        ENDCG
    }
    FallBack "Diffuse"
}