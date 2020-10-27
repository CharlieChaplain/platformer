Shader "Post Processes/SepiaPP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _amount("Amount", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _amount;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                //gets intensity value (black and white basically)
                fixed Y = dot(fixed3(0.299, 0.587, 0.114), col.rgb);

                fixed4 sepia = float4(0.121, -0.054, -0.221, 0.0);
                fixed4 sepOutput = sepia + Y;
                sepOutput.a = col.a;

                fixed4 output = lerp(col, sepOutput, _amount);

                return output;
            }
            ENDCG
        }
    }
}
