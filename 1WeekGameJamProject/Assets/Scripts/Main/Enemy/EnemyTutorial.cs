using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : Enemy
{
	public override void Dead()
	{
		base.Dead();
		TutorialManager.Instance.EnemyDead();
		Debug.Log("死んだ");

	}
}