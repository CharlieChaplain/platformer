Shader "psx/sphereMap" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Environment Texture", 2D) = "white" {}
	}
		SubShader{
			Tags {
				"Queue" = "Geometry"
				"RenderType" = "Opaque"
			}
			LOD 200
		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;

			struct vertInput {
				float4 pos: POSITION;
				float3 normal: NORMAL;
			};

			struct vertOutput {
				float4 pos: SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			vertOutput vert(vertInput input) {
				vertOutput output;

				//vertex snapping
				float4 snapToPixel = UnityObjectToClipPos(input.pos);
				float4 vertex = snapToPixel;
				vertex.xyz = snapToPixel.xyz / snapToPixel.w;
				vertex.x = floor(160 * vertex.x) / 160;
				vertex.y = floor(120 * vertex.y) / 120;
				vertex.xyz *= snapToPixel.w;
				output.pos = vertex;

				//calculating uv
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 viewDir = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.pos).xyz);
				float3 normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
				float3 r = normalize(reflect(viewDir, normalDir));

				float M = 2 * sqrt(pow(r.x, 2.0) + pow(r.y, 2.0) + pow(r.z + 1.0, 2.0));

				float u = (r.x / M) + 0.5;
				float v = (r.y / M) + 0.5;

				output.uv = float2(u, v);
				return output;
			}

			float4 frag(vertOutput input) : COLOR
			{
				return tex2D(_MainTex, input.uv);
			}


			ENDCG
		} }
		FallBack "Diffuse"
}
