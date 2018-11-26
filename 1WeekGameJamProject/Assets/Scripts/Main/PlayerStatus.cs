using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
	public const int ConstParamator = 20;

	public int level;
	public int atk;
	public int def;
	public int spd;
	public int exp;

	public PlayerStatus(bool _isTutorial)
	{
		if (_isTutorial)
			level = 5;
		else
			level = 10;

		exp = 0;
		atk = 1;
		def = 1;
		spd = 1;
	}
}
