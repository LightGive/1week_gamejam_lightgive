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
	private RangeFloat m_rangeDistance;
	[SerializeField]
	private RangeFloat m_rangeForcePower;
	[SerializeField]
	private RangeFloat m_rangeSoundMag;

	[SerializeField]
	private int m_nowNum;

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

	void Start()
	{
		m_jellySprite = this.gameObject.GetComponent<JellySprite>();
		Calc(Operator.Add, 0);
	}

	protected virtual void Update()
	{
		CheckTouch();
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
				m_jellySprite.AddForce(vec * Mathf.Lerp(m_rangeForcePower.MinValue, m_rangeForcePower.MaxValue, m_mainArrow.lerp));
				m_mainArrow.SetActive(false);
			}
		}
	}

	void Calc(Operator _ope, int _num)
	{
		switch (_ope)
		{
			case Operator.Add:
				m_nowNum += _num;
				break;
			case Operator.Sub:
				m_nowNum -= _num;
				break;
			case Operator.Multi:
				m_nowNum *= _num;
				break;
			case Operator.Div:
				if (_num == 0)
				{
					Debug.Log("0除算=>NaN");
				}
				break;
		}

		m_numSPrite.SetNumber(Operator.Add, m_nowNum);
	}

	void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D _col)
	{
		if (Time.time - m_soundPlayTime < 0.2f)
			return;
		var mag = Mathf.Abs(centerObj.GetComponent<Rigidbody2D>().velocity.y);

		if (mag < m_rangeSoundMag.MinValue)
			return;


		mag = Mathf.Clamp(mag, m_rangeSoundMag.MinValue, m_rangeSoundMag.MaxValue);
		var lerp = (mag - m_rangeSoundMag.MinValue) / m_rangeSoundMag.MinValue;
		var vol = Mathf.Lerp(m_rangeSoundMag.MinValue, m_rangeSoundMag.MaxValue, lerp);
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Puyo, vol);
		m_soundPlayTime = Time.time;

	}

	void OnJellyTriggerEnter2D(JellySprite.JellyCollider2D _col)
	{
		if (_col.Collider2D.tag != TagName.Enemy)
			return;

		var enemy = _col.Collider2D.gameObject.GetComponent<Enemy>();
		enemy.Hit();
		Calc(enemy.ope, enemy.num);
	}

	public class PlayerStatus
	{
		public float moveSpeed;
		public int hitPoint;


	}
}
