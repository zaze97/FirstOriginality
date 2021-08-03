Shader "Custom/ShaderLabProperties"
{
Properties{
_Int ("Int",Int) = 2
_Float ("Float",Float) = 1.5
_Range("Range",Range(0.0,5.0)) = 3.0
_Color ("Color",Color) = (1,1,1,1)
_Vector ("Vector",Vector) = (2,3,6,1)
_2D ("2D",2D) = "" {}
_Cube ("Cube",Cube) = "white" {}
_3D ("3D",3D) = "black" {}

}
SubShader{

	Tags{ "RenderType"="Opaque" }//SubShader的Tags标识


	Pass{
	Tags {"LightMode"="ForwardBase"}//Pass的Tags
	Name "MyPassName"//SubShader里Pass代码块的名字
	UsePass "Custom/ShaderLabProperties/MyPassName"//通过shader路径加上Pass的名字可以使用别的Pass
	GrabPass {}//该Pass负责抓取屏幕并将结果存储在一张纹理中
	}

	Pass {
	// Pass的标签和状态设置
　	CGPROGRAM
	// 编译指令，例如：
	#pragma vertex vert
	#pragma fragment frag
	// Cg代码
	ENDCG
	// 其他一些设置
}
}
FallBack "Diffuse"//如果上面的运行不了，就执行此路径的Pass
}