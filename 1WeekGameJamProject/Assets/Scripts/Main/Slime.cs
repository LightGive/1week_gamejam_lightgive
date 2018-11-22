using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
	[SerializeField]
	private MainArrow m_mainArrow;
	[SerializeField]
	private Transform m_eyeTarget;
	[SerializeField]
	private RangeFloat m_rangeDistance;
	[SerializeField]
	private RangeFloat m_rangeForcePower;
	[SerializeField]
	private RangeFloat m_rangeSoundMag;
	[SerializeField]
	private float m_minHitVelocity;
	[SerializeField]
	private float m_soundIntervalMin = 0.2f;
	[SerializeField]
	private float m_velocityIntervalMin = 0.2f;
	[SerializeField]
	private PlayerStatus m_status;
	[SerializeField]
	private GameObject[] m_parabolaDots;

	private JellySprite m_jellySprite;
	private Vector2 m_beginTouchPos;
	private float m_lastSoundPlayTime = 0.0f;
	private float m_lastVelocityTime = 0.0f;
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
	public Vector2 positionVec2 { get { return new Vector2(transform.position.x, transform.position.y); } }

	void Start()
	{
		m_jellySprite = this.gameObject.GetComponent<JellySprite>();
	}

	protected virtual void Update()
	{
		CheckTouch();
		CheckEyeTarget();
	}

	/// <summary>
	/// 見る方向を決める
	/// </summary>
	void CheckEyeTarget()
	{
		if (m_isTouch)
		{
			var tmpPos = SceneMain.Instance.mainCamera.pixelCamera.ScreenToWorldPosition(Input.mousePosition);
			var touchPos = new Vector2(tmpPos.x, tmpPos.y);
			var vec = m_beginTouchPos - touchPos;
			var pos = positionVec2 + vec;
			m_eyeTarget.position = new Vector3(pos.x, pos.y, 0.0f);
		}
		else
		{
			var pos = positionVec2 + m_jellySprite.CentralPoint.Body2D.velocity * 0.5f;
			m_eyeTarget.position = new Vector3(pos.x, pos.y, 0.0f);
		}
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

			//設定によって変更
			switch (SaveManager.Instance.saveData.settingData.settingVelocityGuide)
			{
				case SettingVelocityGuide.None:
					break;

				case SettingVelocityGuide.Parabola:

					//放物線の座標を取得
					var positions = SystemCalc.GetBallisticpredictionPoint(
						CalcVelocity(-vec.normalized),
						transform.position,
						m_parabolaDots.Length,
						0.1f,
						Physics2D.gravity.y * m_jellySprite.m_GravityScale);

					for (int i = 0; i < m_parabolaDots.Length; i++)
					{
						if (!m_parabolaDots[i].activeSelf)
							m_parabolaDots[i].SetActive(true);
						m_parabolaDots[i].transform.position = positions[i];
					}

					break;

				case SettingVelocityGuide.Arrow:

					if (isActive)
					{
						dis = Mathf.Clamp(dis, m_rangeDistance.MinValue, m_rangeDistance.MaxValue);
						var disLerp = (dis - m_rangeDistance.MinValue) / m_rangeDistance.MinValue;
						m_mainArrow.ChangeArrowLengh(disLerp, vec.normalized);
					}

					//表示の変更
					if (m_mainArrow.isActive != isActive)
						m_mainArrow.SetActive(isActive);
					break;
			}
		}
		else if (m_isTouch && Input.GetMouseButtonUp(0))
		{
			//タッチ終了
			m_isTouch = false;

			//設定によって変更
			switch (SaveManager.Instance.saveData.settingData.settingVelocityGuide)
			{
				case SettingVelocityGuide.Arrow:
					//矢印が表示されているか
					if (m_mainArrow.isActive)
					{
						var vec = (m_beginTouchPos - nowTouchPos).normalized;
						m_jellySprite.ChangeVelocity(CalcVelocity(vec));
						m_mainArrow.SetActive(false);
					}
					break;
				case SettingVelocityGuide.Parabola:
					//放物線を非表示にする
					for (int i = 0; i < m_parabolaDots.Length; i++)
					{
						m_parabolaDots[i].SetActive(false);
					}
					break;
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

				//連続で音が鳴らないように
				if (Time.time - m_lastSoundPlayTime < m_soundIntervalMin)
					return;

				//速度を計算
				var mag = Mathf.Abs(centerObj.GetComponent<Rigidbody2D>().velocity.y);
				if (mag < m_rangeSoundMag.MinValue)
					return;

				//サウンドの大きさを計算
				mag = Mathf.Clamp(mag, m_rangeSoundMag.MinValue, m_rangeSoundMag.MaxValue);
				var vol = (mag - m_rangeSoundMag.MinValue) / (m_rangeSoundMag.MaxValue - m_rangeSoundMag.MinValue);

				//SEを鳴らす
				SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Puyo, vol);
				m_lastSoundPlayTime = Time.time;
				break;
		}
	}

	Vector3 CalcVelocity(Vector2 _normalizedVec)
	{
		return _normalizedVec * Mathf.Lerp(m_rangeForcePower.MinValue, m_rangeForcePower.MaxValue, m_mainArrow.lerp) * m_status.moveSpeed;
	}



	/// <summary>
	/// 経験値を付与する
	/// </summary>
	/// <param name="_addExp">Add exp.</param>
	public void AddExp(int _addExp = 0)
	{
		SceneMain.Instance.uIController.sliderExp.AddExp(_addExp);
	}

	/// <summary>
	/// レベルアップ時
	/// </summary>
	public void LevelUp()
	{
		m_status.level++;
		m_status.exp = 0;
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
