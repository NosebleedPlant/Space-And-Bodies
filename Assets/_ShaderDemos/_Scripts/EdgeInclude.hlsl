#ifndef EDGE_INCLUDED
#define EDGE_INCLUDED

float2 sobelSamplePoints[9] =
{
  float2(-1,1),float2(0,1),float2(1,1),
  float2(-1,0),float2(0,0),float2(1,0),
  float2(-1,-1),float2(0,-1),float2(1,-1)
};

float xMatrix[9] =
{
    1, 0, -1,
    2, 0, -2,
    1, 0, -1
};
float yMatrix[9] =
{
    1,2,1,
    0,0,0,
    -1,-2,-1
};

void edgeDetect_float(float2 uv, float thickness, out float o)
{
    float2 sobel = 0;
    [unroll] for(int i =0; i<9;i++)
    {
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv + sobelSamplePoints[i] * thickness);
        sobel += depth * float2(xMatrix[i], yMatrix[i]);
    }
    
    o = length(sobel);
};

#endif