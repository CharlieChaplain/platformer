// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PSXShader"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				half3 normal : TEXCOORD1;
				fixed4 vertex : SV_POSITION;
				half4 color : COLOR0;
				half4 colorFog : COLOR1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			
			v2f vert (appdata_full v)
			{
				float4 shadowColor = (54, 17, 56, 1);
				float4 decShadowColor = (0.2117, 0.0666, 0.2196, 1);

				//polygon snapping
				float snapRange = 0.5;
				
				float4 CamVertex = UnityObjectToClipPos(v.vertex);
				float4 NewVertex = CamVertex;
				NewVertex.xyz = CamVertex.xyz / CamVertex.w;
				NewVertex.x = floor((snapRange * 160) * NewVertex.x) / (snapRange * 160);
				NewVertex.y = floor((snapRange * 120) * NewVertex.y) / (snapRange * 120);
				NewVertex.xyz *= CamVertex.w;
				
				v2f o;
				o.vertex = NewVertex;

				//Vertex Lighting

				float4x4 modelMatrixInverse = unity_WorldToObject;

				float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
				float3 lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));
				float3 diffuseReflection = float3(_LightColor0.xyz)* max(0.0, dot(normalDirection, lightDirection));

				o.color = (float4(diffuseReflection, 1.0) + UNITY_LIGHTMODEL_AMBIENT);

				float distance = length(UnityObjectToViewPos(v.vertex));
				
				//affine texture mapping
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv *= (distance + (NewVertex.w * 1.0)) / distance / 2;
				o.normal = (distance + (NewVertex.w * 1.0)) / distance / 2;
				




				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				_Color = float4(1,0.9098,0.7373,1);

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				
				//add shadow
				col *= i.color;

				//add tint
				//col *= _Color;

				//reduce color depth (I think)
				col = floor(col * 8) / 8;

				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}

	//Transparent
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		Zwrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		Lighting On
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"

		struct v2f
		{
			fixed4 pos: SV_POSITION;
			half4 color: COLOR0;
			half4 colorFog: COLOR1;
			float2 uv: TEXCOORD0;
			half3 normal: TEXCOORD1;
		};

		float4 _MainTex_ST;
		uniform half4 unity_FogStart;
		uniform half4 unity_FogEnd;

		v2f vert(appdata_full v)
		{
			v2f o;

			//handles vertex snapping
			float snapRange = 0.5;
				
			float4 CamVertex = UnityObjectToClipPos(v.vertex);
			float4 NewVertex = CamVertex;
			NewVertex.xyz = CamVertex.xyz / CamVertex.w;
			NewVertex.x = floor((snapRange * 160) * NewVertex.x) / (snapRange * 160);
			NewVertex.y = floor((snapRange * 120) * NewVertex.y) / (snapRange * 120);
			NewVertex.xyz *= CamVertex.w;

			o.pos = NewVertex;

			//Vertex Lighting
			float4x4 modelMatrixInverse = unity_WorldToObject;

			float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
			float3 lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));
			float3 diffuseReflection = float3(_LightColor0.xyz)* max(0.0, dot(normalDirection, lightDirection));

			o.color = (float4(diffuseReflection, 1.0) + UNITY_LIGHTMODEL_AMBIENT);

			float distance = length(UnityObjectToViewPos(v.vertex));

			//affine texture mapping
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv *= (distance + (NewVertex.w * 1.0)) / distance / 2;
			o.normal = (distance + (NewVertex.w * 1.0)) / distance / 2;

			//Fog
			float4 fogColor = unity_FogColor;
			float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
			o.normal.g = fogDensity;
			o.normal.b = 1;

			o.colorFog = fogColor;
			o.colorFog.a = clamp(fogDensity, 0, 1);

			//cut out polys
			if (distance > unity_FogStart.z + unity_FogColor.a * 255){
				o.pos.w = 0;
			}


			return o;
		}

		sampler2D _MainTex;

		float4 frag(v2f IN) : COLOR
		{
			half4 color = tex2D(_MainTex, IN.uv/ IN.normal.r) * IN.color;
			//fixed4 color = c*(IN.colorFog.a);
			//color.rgb += IN.colorFog.rgb*(1-IN.colorFog.a);
			color.rgb *= color.a;
			return color;
		}
			ENDCG


		}
	}
}
