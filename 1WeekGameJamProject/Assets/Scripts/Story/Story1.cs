using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story1 : StoryBase
{
	[SerializeField]
	private CloudInfo[] m_cloudInfos;
	[SerializeField]
	private RangeFloat m_cloudSpeedRange;

	[SerializeField]
	private float m_rightEdge;
	[SerializeField]
	private float m_createLeftEdge;


	void Update()
	{
		for (int i = 0; i < m_cloudInfos.Length; i++)
		{
			var info = m_cloudInfos[i];
			info.cloudObj.transform.localPosition += new Vector3(info.cloudSpeed, 0.0f, 0.0f) * Time.deltaTime;

			if (info.cloudObj.transform.localPosition.x > m_rightEdge)
			{
				info.cloudObj.transform.localPosition = new Vector3(m_createLeftEdge, info.basePosY, 1.0f);
				info.cloudSpeed = m_cloudSpeedRange.RandomValue;
			}
		}
	}


	[System.Serializable]
	public class CloudInfo
	{
		public GameObject cloudObj;
		public float cloudSpeed;
		public float basePosY;
	}
}
