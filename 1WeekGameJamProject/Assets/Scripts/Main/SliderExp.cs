using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderExp : MonoBehaviour
{
	[SerializeField]
	private Slider m_slider;
	[SerializeField]
	private float m_addTime = 1.0f;
	[SerializeField]
	private AnimationCurve m_animCurve;

	private bool m_isAddProduction;
	private int m_addExp;
	private float m_timeCount;
	private Queue<AddInfo> m_addExpQueue = new Queue<AddInfo>();
	private AddInfo m_addInfo;

	private void Update()
	{
		if (!m_isAddProduction)
			return;

		m_timeCount += Time.deltaTime;
		var lerp = Mathf.Clamp01(m_timeCount / m_addTime);
		m_slider.value = m_animCurve.Evaluate(lerp);
	}

	public void AddExp(int _addExp)
	{
		while (_addExp > 0.0f)
		{
			var currentExp = SceneMain.Instance.slime.playerStatus.exp;
			var maxExp = SceneMain.Instance.slime.playerStatus.maxExp;

			var realAdd = _addExp;
			if (_addExp > (maxExp - currentExp))
			{
				realAdd = maxExp - currentExp;
			}

			m_addExpQueue.Enqueue(new AddInfo(currentExp, (currentExp + realAdd)));

			//経験値がMaxかどうかの判定
			if ((currentExp + realAdd) >= maxExp)
			{
				//レベルアップ
				SceneMain.Instance.slime.LevelUp();
			}
			_addExp -= realAdd;
		}

		CheckQueue();
	}

	public void CheckQueue()
	{
		if (m_addExpQueue.Count <= 0)
			return;

		m_addInfo = m_addExpQueue.Dequeue();
		m_isAddProduction = true;
		m_timeCount = 0.0f;
	}

	public class AddInfo
	{
		public int fromExp;
		public int toExp;

		public AddInfo(int _fromExp, int _toExp)
		{
			fromExp = _fromExp;
			toExp = _toExp;
		}
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
