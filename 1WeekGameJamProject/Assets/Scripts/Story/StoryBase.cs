using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBase : MonoBehaviour
{
	[SerializeField, Multiline]
	private DisplayInfo[] m_displayTextInfos;

	public DisplayInfo[] displayTextInfos { get { return m_displayTextInfos; } }

	[System.Serializable]
	public class DisplayInfo
	{
		public string nameText;
		public string displayText;
	}
}
