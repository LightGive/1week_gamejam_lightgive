using UnityEngine;

[ExecuteInEditMode]
public class PixelCamera2D : MonoBehaviour
{
	[SerializeField]
	private int baseWidth = 256;
	[SerializeField]
	private int baseHeight = 144;
	[SerializeField]
	private PixelCamera2DBehaviour behaviour;
	[SerializeField]
	private MeshRenderer quad;
	[SerializeField]
	private Camera pixelCamera;
	[SerializeField]
	private Camera pixelCameraRenderer;

	private PixelCamera2DBehaviour previousBehaviour;
	private int previousWidth = 0;
	private int previousHeight = 0;

	public int BaseWidth { get { return baseWidth; } }
	public int BaseHeight { get { return baseHeight; } }


	private void Update()
	{
		if (Screen.width != previousWidth || Screen.height != previousHeight || previousBehaviour != behaviour)
		{
			UpdatePreviousValues();
			UpdateCamera();
		}
	}

	public void SetRenderTexture(RenderTexture renderTexture)
	{
		//pixelCameras[0].targetTexture = renderTexture;
		quad.sharedMaterial.mainTexture = renderTexture;
	}

	private void UpdateCamera()
	{
		if (behaviour == PixelCamera2DBehaviour.BestPixelPerfectFit)
		{
			BestFitBehaviour();
		}
		else if (behaviour == PixelCamera2DBehaviour.ScaleToFit)
		{
			ScaleBehaviour();
		}
	}

	private void BestFitBehaviour()
	{
		int nearestWidth = Screen.width / baseWidth * baseWidth;
		int nearestHeight = Screen.height / baseHeight * baseHeight;

		int xScaleFactor = nearestWidth / baseWidth;
		int yScaleFactor = nearestHeight / baseHeight;

		int scaleFactor = yScaleFactor < xScaleFactor ? yScaleFactor : xScaleFactor;

		float heightRatio = (baseHeight * (float)scaleFactor) / Screen.height;
		quad.transform.localScale = new Vector3(baseWidth / (float)baseHeight * heightRatio, 1f * heightRatio, 1f);

		// Offset the camera rect in odd screen sizes to prevent subpixel issues.
		pixelCameraRenderer.rect = new Rect(GetCameraRectOffset(Screen.width), GetCameraRectOffset(Screen.height), pixelCameraRenderer.rect.width, pixelCameraRenderer.rect.height);
	}

	private void ScaleBehaviour()
	{
		float targetAspectRatio = baseWidth / (float)baseHeight;
		float windowAspectRatio = Screen.width / (float)Screen.height;
		float scaleHeight = windowAspectRatio / targetAspectRatio;

		if (scaleHeight < 1f)
		{
			quad.transform.localScale = new Vector3(targetAspectRatio * scaleHeight, scaleHeight, 1f);
		}
		else
		{
			quad.transform.localScale = new Vector3(targetAspectRatio, 1f, 1f);
		}
	}

	private void UpdatePreviousValues()
	{
		previousWidth = Screen.width;
		previousHeight = Screen.height;
		previousBehaviour = behaviour;
	}

	private float GetCameraRectOffset(int size)
	{
		return size % 2 == 0 ? 0 : 1f / size;
	}

	public Vector3 ScreenToWorldPosition(Vector3 screenPosition)
	{
		int targetWidth = baseWidth;
		int targetHeight = baseHeight;

		if (behaviour == PixelCamera2DBehaviour.BestPixelPerfectFit)
		{
			targetWidth = Screen.width / baseWidth * baseWidth;
			targetHeight = Screen.height / baseHeight * baseHeight;
		}
		else if (behaviour == PixelCamera2DBehaviour.ScaleToFit)
		{
			targetWidth = Screen.width;
			targetHeight = Screen.height;
		}

		float xScaleFactor = (float)targetWidth / baseWidth;
		float yScaleFactor = (float)targetHeight / baseHeight;
		float scalefactor = Mathf.Min(xScaleFactor, yScaleFactor);

		targetWidth = (int)(baseWidth * scalefactor);
		targetHeight = (int)(baseHeight * scalefactor);

		Vector3 offset = new Vector3(
			(Screen.width - targetWidth) / 2,
			(Screen.height - targetHeight) / 2,
			0.0f);

		Vector3 correctedPosition = (screenPosition - offset) / scalefactor;
		return pixelCamera.ScreenToWorldPoint(correctedPosition);
	}



	/// <summary>
	/// タッチしたワールド座標を返す
	/// </summary>
	/// <returns>The touch world position.</returns>
	public Vector2 GetTouchWorldPos()
	{
		var inputPos = Input.mousePosition;
		var viewportPos = new Vector2(inputPos.x / Screen.width, inputPos.y / Screen.height);
		return pixelCamera.ViewportToWorldPoint(viewportPos);
	}

	/// <summary>
	/// タッチしたところからレイを飛ばしてワールド座標を返す
	/// </summary>
	/// <returns><c>true</c>, if touch hit position was gotten, <c>false</c> otherwise.</returns>
	/// <param name="_hitPos">Hit position.</param>
	public bool GetTouchHitPos(out Vector3 _hitPos)
	{
		RaycastHit hit;
		var inputPos = Input.mousePosition;
		var viewportPos = new Vector2(inputPos.x / Screen.width, inputPos.y / Screen.height);
		var ray = pixelCamera.ViewportPointToRay(viewportPos);
		_hitPos = Vector3.zero;

		if (Physics.Raycast(ray, out hit))
		{
			_hitPos = hit.point;
			return true;
		}
		else
		{
			return false;
		}
	}
}