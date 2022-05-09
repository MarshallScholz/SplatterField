#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

//sample from relative pixel (0, 0)
static float2 sobelSamplePoints[9] =
{
	float2(-1, 1), float2(0, 1), float2(1, 1),
	float2(-1, 0), float2(0, 0), float2(1, 1),
	float2(-1, -1), float2(0, -1), float2(1, -1)
};

//Weights for x component
static float sobelXMatrix[9] =
{
	1, 0, -1,
	2, 0, -2,
	1, 0, -1
};

//Weights for y component
static float sobelYMatrix[9] =
{
	1, 2, 1,
	0, 0, 0,
	-1, -2, -1
};

//Runs sobel algorithm over the depth texture
//			'_float' tells the shader what type of numbers it uses.
void DepthSobel_float(float2 UV, float Thickness,  SceneDepthTexture, out float Out)
{
	float sobel = 0;
	
	[unroll] for (int i = 0; i < 9; i++)
	{
		float depth = LOAD_TEXTURE2D_LOD(SceneDepthTexture,UV + sobelSamplePoints[i] * Thickness, 0);
		//float depth = _CameraDepthTexutre(UV + sobelSamplePoints[i] * Thickness);
					//multiply the depths by the weights
		sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
		

	}
	//get the final sobel value
	Out = length(sobel);

}
#endif