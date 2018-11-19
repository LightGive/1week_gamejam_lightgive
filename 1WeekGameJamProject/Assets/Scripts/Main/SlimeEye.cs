using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEye : MonoBehaviour
{
	[SerializeField]
	private GameObject m_eye;
	[SerializeField]
	private Vector2 m_eyeRange;
	[SerializeField]
	private float m_minDistance;

	void Start()
	{

	}

	void Update()
	{
		LookMouse();
	}

	void SetCenterPos()
	{
		m_eye.transform.localPosition = new Vector3(0.0f, 0.0f, -1.0f);
	}

	void LookMouse()
	{
		var mousePos = (Vector2)SceneMain.Instance.mainCamera.pixelCamera.ScreenToWorldPosition(Input.mousePosition);
		var eyeCenter = (Vector2)transform.position;
		var dis = Vector2.Distance(mousePos, eyeCenter);

		var vec = (mousePos - eyeCenter).normalized;
		var pos = (dis < m_minDistance) ? Vector2.zero : vec * m_eyeRange;
		m_eye.transform.localPosition = new Vector3(pos.x, pos.y, -1.0f);
	}
}
