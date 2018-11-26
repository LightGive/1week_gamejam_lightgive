using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSlime : MonoBehaviour
{
	[SerializeField]
	private Transform m_eyeTarget;
	[SerializeField]
	private RangeFloat m_rangePower;
	[SerializeField]
	private RangeFloat m_rangeWaitTime;
	[SerializeField]
	private RangeFloat m_rangeSoundMag;
	[SerializeField]
	private float m_soundIntervalMin = 0.2f;

	private JellySprite m_jellySprite;
	private float m_timeCount;
	private float m_waitTime;
	private float m_lastSoundPlayTime = 0.0f;


	public Vector2 positionVec2 { get { return new Vector2(transform.position.x, transform.position.y); } }


	void Start()
	{
		m_jellySprite = this.gameObject.GetComponent<JellySprite>();

	}

	void Update()
	{
		CheckEyeTarget();
		JumpTimeCount();
	}

	void JumpTimeCount()
	{
		m_timeCount += Time.deltaTime;
		if (m_timeCount > m_waitTime)
		{
			Jump();
			m_waitTime = m_rangeWaitTime.RandomValue;
			m_timeCount = 0.0f;
		}
	}

	/// <summary>
	/// 見る方向を決める
	/// </summary>
	void CheckEyeTarget()
	{
		var pos = positionVec2 + m_jellySprite.CentralPoint.Body2D.velocity * 0.5f;
		m_eyeTarget.position = new Vector3(pos.x, pos.y, 0.0f);
	}

	/// <summary>
	/// ジャンプする
	/// </summary>
	void Jump()
	{
		var ran = Random.Range(-1.0f, 1.0f);
		var randomVec = new Vector2(Mathf.Sin(ran), Mathf.Cos(ran)).normalized;
		m_jellySprite.ChangeVelocity(randomVec * m_rangePower.RandomValue);
	}


	/// <summary>
	/// 当たった時
	/// </summary>
	/// <param name="_col">Col.</param>
	void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D _col)
	{
		switch (_col.Collision2D.gameObject.tag)
		{
			case TagName.StageObject:

				//連続で音が鳴らないように
				if (Time.time - m_lastSoundPlayTime < m_soundIntervalMin)
					return;

				//速度を計算
				var mag = Mathf.Abs(m_jellySprite.CentralPoint.GameObject.GetComponent<Rigidbody2D>().velocity.y);
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
}
