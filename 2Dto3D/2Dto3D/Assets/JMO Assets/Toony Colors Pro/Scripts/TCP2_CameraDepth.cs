// Toony Colors Pro+Mobile 2
// (c) 2014-2017 Jean Moreno

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Makes the Camera render a depth texture.
// This is needed for some water shaders that use depth-based effects such as edge intersection.

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class TCP2_CameraDepth : MonoBehaviour
{
	public bool RenderDepth = true;

	void OnEnable()
	{
		SetCameraDepth();
	}

	void OnValidate()
	{
		SetCameraDepth();
	}

	void SetCameraDepth()
	{
		var cam = this.GetComponent<Camera>();
		if (RenderDepth)
			cam.depthTextureMode |= DepthTextureMode.Depth;
		else
			cam.depthTextureMode &= ~DepthTextureMode.Depth;
	}
}
