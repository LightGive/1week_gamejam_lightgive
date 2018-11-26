using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEndroll : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_scrollContent;
	[SerializeField]
	private float m_normalScrollSpeed;
	[SerializeField]
	private float m_fastScrollSpeed;
	[SerializeField]
	private bool m_isFast;
	[SerializeField]
	private RangeFloat m_rangeScroll;

	private bool m_isScroll;

	private float scrollSpeed { get { return (m_isFast) ? m_fastScrollSpeed : m_normalScrollSpeed; } }

	void Start()
	{
		SaveManager.Instance.saveData.userData.clearCount++;
		SaveManager.Instance.Save();
		m_isScroll = true;

		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Endroll);
		m_scrollContent.anchoredPosition = new Vector2(0.0f, m_rangeScroll.MinValue);
	}

	void Update()
	{
		if (!m_isScroll)
			return;

		var speed = (Input.GetMouseButton(0)) ? m_fastScrollSpeed : m_normalScrollSpeed;
		var pos = m_scrollContent.anchoredPosition.y;
		pos = Mathf.Clamp(pos + speed * Time.deltaTime, m_rangeScroll.MinValue, m_rangeScroll.MaxValue);
		m_scrollContent.anchoredPosition = new Vector2(0.0f, pos);

		if (pos <= m_rangeScroll.MinValue)
		{
			Debug.Log("終了");
		}
	}

	public void OnButtonDownBackTitle()
	{
		TransitionManager.Instance.LoadScene(SceneName.Title);
	}
}
