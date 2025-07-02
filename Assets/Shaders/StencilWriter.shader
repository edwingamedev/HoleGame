Shader "Custom/StencilWriter"
{
    SubShader
    {
        Tags
        {
            "Queue" = "Geometry-1"
        }
        ColorMask 0
        ZWrite On

        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
        }

        Pass {}
    }
}