using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToLinkButton : MonoBehaviour
{
	public void OnButtonDownLink(string _link)
	{
		Application.OpenURL(_link);
	}
}
