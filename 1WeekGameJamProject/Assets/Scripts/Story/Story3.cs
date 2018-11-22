using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Story3 : StoryBase
{
	[SerializeField]
	private PlayableDirector m_director;

	public void OnThunderFalls()
	{
		m_director.Play();
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Thunder, 1.0f, 0.5f);
		TransitionManager.Instance.Flash(0.0f, 0.1f);
	}
}
