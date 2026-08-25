Shader "GameEditor/VegetationShader" {
Properties {
 _Color ("Color", Color) = (1,1,1,1)
 _MainTex ("Texture", 2D) = "" {}
}
SubShader { 
 Pass {
  Lighting On
  Material {
   Ambient [_Color]
   Diffuse [_Color]
  }
  Cull Off
  SetTexture [_MainTex] { combine texture * primary double }
 }
}
}