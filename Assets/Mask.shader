Shader "Custom/Mask3D"
{

  SubShader
  {
	  Tags {"Queue" = "Transparent+1"}	 

    ColorMask 0
    ZWrite On

    Pass{}
  }

}
