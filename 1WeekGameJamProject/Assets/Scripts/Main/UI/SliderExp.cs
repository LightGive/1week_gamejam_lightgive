using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderExp : MonoBehaviour
{
	[SerializeField]
	private Slider m_slider;
	[SerializeField]
	private TextMeshProUGUI m_textLevelTitle;
	[SerializeField]
	private TextMeshProUGUI m_textLevel;
	[SerializeField]
	private TextMeshProUGUI m_textExpTitle;
	[SerializeField]
	private TextMeshProUGUI m_textExp;
	[SerializeField]
	private Image m_sliderBar;
	[SerializeField]
	private Image m_sliderWindow;

	[SerializeField]
	private float m_addTime = 1.0f;
	[SerializeField]
	private AnimationCurve m_animCurve;
	[SerializeField]
	private Color[] m_tenColors;


	private bool m_isAddProduction;
	private int m_displayLevel;
	private int m_addExp;
	private float m_timeCount;
	private Queue<AddInfo> m_addExpQueue = new Queue<AddInfo>();
	private AddInfo m_addInfo;


	private void Start()
	{

	}

	private void Update()
	{
		if (!m_isAddProduction)
			return;

		m_timeCount += Time.deltaTime;
		var lerp = m_animCurve.Evaluate(Mathf.Clamp01(m_timeCount / m_addTime));
		var nowVal = Mathf.Lerp(m_addInfo.fromExp, m_addInfo.toExp, lerp);
		var sliderVal = nowVal / m_addInfo.maxExp;
		m_textExp.text = (m_addInfo.maxExp * sliderVal).ToString("0") + " / " + m_addInfo.maxExp.ToString("0");
		m_slider.value = sliderVal;

		if (m_timeCount > m_addTime)
		{
			if (m_addInfo.maxExp == m_addInfo.toExp)
			{
				SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.LevelUp);

				m_displayLevel++;
				m_textLevel.text = m_displayLevel.ToString("0");
				m_slider.value = 0.0f;
				ChangeColor();
			}
			m_isAddProduction = false;
			CheckQueue();
		}
	}

	public void SetLevel()
	{
		m_displayLevel = SceneMain.Instance.mainSlime.playerStatus.level;
		m_slider.value = Mathf.Clamp01((float)SceneMain.Instance.mainSlime.playerStatus.exp / 100);
		m_textLevel.text = m_displayLevel.ToString("0");
		ChangeColor();
	}

	public void AddExp(int _addExp)
	{
		while (_addExp > 0.0f)
		{
			var currentExp = SceneMain.Instance.mainSlime.playerStatus.exp;
			var maxExp = 100;

			var realAdd = _addExp;
			if (_addExp > (maxExp - currentExp))
			{
				realAdd = maxExp - currentExp;
			}

			m_addExpQueue.Enqueue(new AddInfo(currentExp, (currentExp + realAdd), maxExp));
			SceneMain.Instance.mainSlime.playerStatus.exp += realAdd;

			//経験値がMaxかどうかの判定
			if ((currentExp + realAdd) >= maxExp)
			{
				//レベルアップ
				SceneMain.Instance.mainSlime.LevelUp();
			}
			_addExp -= realAdd;
		}

		CheckQueue();
	}


	public void CheckQueue()
	{
		if (m_isAddProduction)
			return;
		if (m_addExpQueue.Count <= 0)
			return;

		m_addInfo = m_addExpQueue.Dequeue();
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.ExpUp);

		m_isAddProduction = true;
		m_timeCount = 0.0f;
	}

	public class AddInfo
	{
		public int fromExp;
		public int toExp;
		public int maxExp;

		public AddInfo(int _fromExp, int _toExp, int _maxExp)
		{
			fromExp = _fromExp;
			toExp = _toExp;
			maxExp = _maxExp;
		}
	}

	void ChangeColor()
	{
		var isTen = m_displayLevel % 10 == 0;
		int colorIdx = 0;
		if (isTen)
		{
			colorIdx = Mathf.Clamp(m_displayLevel / 10, 0, m_tenColors.Length - 1);
		}

		m_textExpTitle.color = m_tenColors[colorIdx];
		m_textExp.color = m_tenColors[colorIdx];
		m_sliderBar.color = m_tenColors[colorIdx];
		m_sliderWindow.color = m_tenColors[colorIdx];
		m_textLevelTitle.color = m_tenColors[colorIdx];
		m_textLevel.color = m_tenColors[colorIdx];
	}

	//private IEnumerator _AddExpProduction(int _addExp)
	//{

	//	var diff = m_status.maxExp - m_status.exp;
	//	var timeCnt = 0.0f;
	//	var sliderTime = 1.0f;

	//	do
	//	{
	//		timeCnt = 0.0f;
	//		m_status.exp += _addExp;

	//		//レベルアップする場合
	//		while (timeCnt <= sliderTime)
	//		{
	//			timeCnt += Time.deltaTime;
	//			var lerp = Mathf.Clamp01(timeCnt / sliderTime);
	//			SceneMain.Instance.uIController.sliderExp.value = m_animCurve.Evaluate(lerp);
	//			yield return new WaitForEndOfFrame();
	//		}

	//		if (m_status.exp >= m_status.maxExp)
	//		{
	//			m_status.exp = 0;
	//			LevelUp();
	//		}

	//		yield return new WaitForSeconds(1.0f);

	//		_addExp -= diff;

	//	} while (diff < _addExp);
	//}

}
