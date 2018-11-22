using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story2 : StoryBase
{
	public void OnAppearDevil()
	{
		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Story2);
	}

	public void OnStopBGM()
	{
		SimpleSoundManager.Instance.StopBGM();
	}
}
