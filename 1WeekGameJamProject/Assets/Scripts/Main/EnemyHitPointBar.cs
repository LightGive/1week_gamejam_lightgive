using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitPointBar : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer m_spRendererBar;
	[SerializeField]
	private SpriteRenderer m_spRendererWindow;

	public void SetBarLerp(float _lerp)
	{
		var width = m_spRendererWindow.size.x;
		m_spRendererBar.transform.localScale = new Vector3((width - 2) * _lerp, 1.0f);
	}

}
