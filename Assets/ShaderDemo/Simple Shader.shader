// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Unity Shaders Book/Chapter 5/Simple Shader" {
SubShader {
Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
struct a2v { 
float4 vertex:POSITION;
float3 normal:NORMAL;
float4 texcoord:TEXCOORD0;
};
// ʹ��һ���ṹ�������嶥����ɫ�������
struct v2f {
// SV_POSITION�������Unity��pos������˶����ڲü��ռ��е�λ����Ϣ
float4 pos:SV_POSITION;
// COLOR0����������ڴ洢��ɫ��Ϣ
fixed3 color:COLOR0;
};
v2f vert(a2v v):SV_POSITION{
// ��������ṹ
v2f o;
o.pos=UnityObjectToClipPos(v.vertex);
// v.normal�����˶���ķ��߷����������Χ��[-1.0��1.0]
// ����Ĵ���ѷ�����Χӳ�䵽��[0.0��1.0]
// �洢��o.color�д��ݸ�ƬԪ��ɫ��
o.color=v.normal*0.5+fixed3(0.5,0.5,0.5);
return o;
}
fixed4 frag(v2f i):SV_target{
// ����ֵ���i.color��ʾ����Ļ��
return fixed4(i.color��1.0);
}
ENDCG
}
}
}