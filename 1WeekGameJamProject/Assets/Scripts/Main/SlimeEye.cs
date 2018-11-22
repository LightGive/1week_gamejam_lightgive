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
	[SerializeField]
	private Transform m_lookTarget;
	[SerializeField]
	private Transform m_eyeCenter;

	public Transform lookTarget
	{
		get { return m_lookTarget; }
		set { m_lookTarget = value; }
	}

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
		var lookPos = (Vector2)m_lookTarget.position;
		var eyeCenter = (Vector2)m_eyeCenter.position;
		var dis = Vector2.Distance(lookPos, eyeCenter);

		var vec = (lookPos - eyeCenter).normalized;
		var pos = (dis < m_minDistance) ? Vector2.zero : vec * m_eyeRange;
		m_eye.transform.localPosition = new Vector3(pos.x, pos.y, -1.0f);
	}
}
