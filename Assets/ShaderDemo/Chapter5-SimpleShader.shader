// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unity Shaders Book/Chapter 5/Simple Shader" {
SubShader {
Pass {
	CGPROGRAM
#pragma vertex vert
#pragma fragment frag
			// ʹ��һ���ṹ�������嶥����ɫ��������
			struct a2v{
			// POSITION�������Unity����ģ�Ϳռ�Ķ����������vertex����
			float4 vertex:POSITION;
			// NORMAL�������Unity����ģ�Ϳռ�ķ��߷������normal����
			float3 normal:NORMAL;
			// TEXCOORD0�������Unity����ģ�͵ĵ�һ�������������texcoord����
			float4 texcoord:TEXCOORD0;
			};
			float4 vert(a2v v):SV_POSITION {
				// ʹ��v.vertex������ģ�Ϳռ�Ķ�������
				return UnityObjectToClipPos (v.vertex);
			}
			fixed4 frag():SV_Target {
				return fixed4(1.0,1.0,1.0,1.0);
			}



ENDCG
}
}
}