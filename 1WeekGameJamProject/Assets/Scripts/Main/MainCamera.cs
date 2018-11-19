using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	[SerializeField]
	private PixelCamera2D m_pixelCamera;

	public PixelCamera2D pixelCamera { get { return m_pixelCamera; } }

	public void ShakeCamera()
	{
	}
}
