using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log(name + "Game Object Clicked！");
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log(name + "Game Object PointerDown");
	}
}


