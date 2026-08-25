Shader "GameEditor/LandscapeShader" {
Properties {
 _Color ("Color", Color) = (1,1,1,1)
 _MainTex ("Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Background-1000" "IGNOREPROJECTOR"="True" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Background-1000" "IGNOREPROJECTOR"="True" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZWrite Off
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
}
}