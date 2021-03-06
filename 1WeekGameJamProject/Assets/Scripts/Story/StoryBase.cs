﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryBase : MonoBehaviour
{
	[SerializeField]
	private DisplayInfo[] m_displayTextInfos;

	public DisplayInfo[] displayTextInfos { get { return m_displayTextInfos; } }

	[System.Serializable]
	public class DisplayInfo
	{
		public string nameText;
		[Multiline]
		public string displayText;

		public UnityEvent eventAct;
	}
}
