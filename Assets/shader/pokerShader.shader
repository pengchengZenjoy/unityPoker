// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/pokerShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_MainBackTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
		LOD 200
		Pass{
			Blend  SrcAlpha OneMinusSrcAlpha
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainBackTex;

			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			} ;
			v2f vert (appdata_base v)
			{
			    v2f o;
			   	o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
			    return o;
			}
			float4 frag (v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainBackTex,i.uv);
			    float4 outp = texCol;
			    return outp;
			}
			ENDCG
		}
		Pass{
			Blend  SrcAlpha OneMinusSrcAlpha
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			} ;
			v2f vert (appdata_base v)
			{
			    v2f o;
			   	o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
			    return o;
			}
			float4 frag (v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex,i.uv);
			    float4 outp = texCol;
			    return outp;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
