using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class StatusUI : MonoBehaviour
{
	[SerializeField]
	private Image m_imageAtkBar;
	[SerializeField]
	private Image m_imageDefBar;
	[SerializeField]
	private Image m_imageSpdBar;
	[SerializeField]
	private TextMeshProUGUI m_textAtk;
	[SerializeField]
	private TextMeshProUGUI m_textDef;
	[SerializeField]
	private TextMeshProUGUI m_textSpd;

	[SerializeField]
	private RangeFloat m_rangeBarWidth;
	[SerializeField]
	private int m_maxVal;

	public void SetStatus(PlayerStatus _status)
	{
		m_textAtk.text = _status.atk.ToString("0");
		m_textDef.text = _status.def.ToString("0");
		m_textSpd.text = _status.spd.ToString("0");


		var a = Mathf.Clamp(_status.atk, 0, m_maxVal);
		var d = Mathf.Clamp(_status.def, 0, m_maxVal);
		var s = Mathf.Clamp(_status.spd, 0, m_maxVal);

		var lerpA = (float)a / m_maxVal;
		var lerpD = (float)d / m_maxVal;
		var lerpS = (float)s / m_maxVal;

		m_imageAtkBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(m_rangeBarWidth.MinValue, m_rangeBarWidth.MaxValue, lerpA), m_imageAtkBar.rectTransform.sizeDelta.y);
		m_imageDefBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(m_rangeBarWidth.MinValue, m_rangeBarWidth.MaxValue, lerpD), m_imageDefBar.rectTransform.sizeDelta.y);
		m_imageSpdBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(m_rangeBarWidth.MinValue, m_rangeBarWidth.MaxValue, lerpS), m_imageSpdBar.rectTransform.sizeDelta.y);

	}
}
