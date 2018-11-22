using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
	[SerializeField]
	private DisplayNumberSprite m_numSPrite;
	[SerializeField]
	private MainArrow m_mainArrow;
	[SerializeField]
	private AnimationCurve m_animCurve;
	[SerializeField]
	private RangeFloat m_rangeDistance;
	[SerializeField]
	private RangeFloat m_rangeForcePower;
	[SerializeField]
	private RangeFloat m_rangeSoundMag;
	[SerializeField]
	private float m_minHitVelocity;
	[SerializeField]
	private PlayerStatus m_status;

	private JellySprite m_jellySprite;
	private Vector2 m_beginTouchPos;
	private float m_soundPlayTime = 0.0f;
	private bool m_isTouch = false;

	/// <summary>
	/// キャラの中心のオブジェクト
	/// </summary>
	public GameObject centerObj
	{
		get { return m_jellySprite.CentralPoint.GameObject; }
	}
	public int level
	{
		get { return m_status.level; }
	}
	public PlayerStatus playerStatus { get { return m_status; } }

	void Start()
	{
		m_jellySprite = this.gameObject.GetComponent<JellySprite>();
		//Calc(Operator.Add, 0);
	}

	protected virtual void Update()
	{
		CheckTouch();
	}

	public void AddExp(int _addExp = 0)
	{
		SceneMain.Instance.uIController.sliderExp.AddExp(_addExp);
	}

	public void LevelUp()
	{
		m_status.level++;
		m_status.exp = 0;
	}

	void CheckTouch()
	{
		var pos = SceneMain.Instance.mainCamera.pixelCamera.ScreenToWorldPosition(Input.mousePosition);
		var nowTouchPos = new Vector2(pos.x, pos.y);

		if (!m_isTouch && Input.GetMouseButtonDown(0))
		{
			//タッチ開始
			m_isTouch = true;
			m_beginTouchPos = nowTouchPos;
		}
		else if (m_isTouch && Input.GetMouseButton(0))
		{
			//タッチ中
			var vec = nowTouchPos - m_beginTouchPos;
			var dis = vec.magnitude;
			var isActive = (dis >= m_rangeDistance.MinValue);

			if (m_mainArrow.isActive != isActive)
				m_mainArrow.SetActive(isActive);

			if (isActive)
			{
				dis = Mathf.Clamp(dis, m_rangeDistance.MinValue, m_rangeDistance.MaxValue);
				var disLerp = (dis - m_rangeDistance.MinValue) / m_rangeDistance.MinValue;
				m_mainArrow.ChangeArrowLengh(disLerp, vec.normalized);
			}
		}
		else if (m_isTouch && Input.GetMouseButtonUp(0))
		{
			//タッチ終了
			m_isTouch = false;

			//矢印が表示されているかの判定
			if (m_mainArrow.isActive)
			{
				var vec = (m_beginTouchPos - nowTouchPos).normalized;
				m_jellySprite.ChangeVelocity(vec * Mathf.Lerp(m_rangeForcePower.MinValue, m_rangeForcePower.MaxValue, m_mainArrow.lerp) * m_status.moveSpeed);
				m_mainArrow.SetActive(false);
			}
		}
	}

	/// <summary>
	/// 当たった時
	/// </summary>
	/// <param name="_col">Col.</param>
	void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D _col)
	{
		switch (_col.Collision2D.gameObject.tag)
		{
			case TagName.Enemy:

				var vec = centerObj.GetComponent<Rigidbody2D>().velocity;

				if (vec.magnitude < m_minHitVelocity)
					return;

				var enemy = _col.Collision2D.gameObject.GetComponent<Enemy>();
				enemy.Hit(m_status.atk);
				break;

			case TagName.StageObject:

				//Do not ring twice in a row
				if (Time.time - m_soundPlayTime < 0.2f)
					return;

				//Calc hit Speed
				var mag = Mathf.Abs(centerObj.GetComponent<Rigidbody2D>().velocity.y);
				if (mag < m_rangeSoundMag.MinValue)
					return;

				//Calc sound volume
				mag = Mathf.Clamp(mag, m_rangeSoundMag.MinValue, m_rangeSoundMag.MaxValue);
				var vol = (mag - m_rangeSoundMag.MinValue) / (m_rangeSoundMag.MaxValue - m_rangeSoundMag.MinValue);

				//Play SE
				SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Puyo, vol);
				m_soundPlayTime = Time.time;
				break;
		}


	}

	[System.Serializable]
	public class PlayerStatus
	{
		public int level;
		public int atk;
		public int exp;
		public int maxExp;
		public float moveSpeed;
	}
}
