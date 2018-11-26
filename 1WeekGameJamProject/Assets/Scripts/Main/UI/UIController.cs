using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	[SerializeField]
	private GameObject m_topUI;
	[SerializeField]
	private StatusUI m_statusUI;
	[SerializeField]
	private SliderExp m_sliderExp;
	[SerializeField]
	private Slider m_sliderTime;

	public GameObject topUI { get { return m_topUI; } }
	public SliderExp sliderExp { get { return m_sliderExp; } }
	public Slider sliderTime { get { return m_sliderTime; } }
	public StatusUI statusUI { get { return m_statusUI; } }

	private void Start()
	{
		if (GameManager.Instance.isTutorial)
		{
			m_topUI.SetActive(false);
			m_statusUI.gameObject.SetActive(false);
			m_sliderExp.gameObject.SetActive(false);
		}
	}
}
