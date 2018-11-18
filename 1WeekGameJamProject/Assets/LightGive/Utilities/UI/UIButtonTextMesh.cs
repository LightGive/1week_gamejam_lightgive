using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


[RequireComponent(typeof(Button))]
[ExecuteInEditMode]
public class UIButtonTextMesh : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	[SerializeField]
	private TextMeshProUGUI m_textButton;
	[SerializeField]
	private Button m_mainButton;
	[SerializeField]
	private Vector2 m_pressTextOffsetPos;

	private bool m_isPointerDown = false;
	private Vector2 m_defaultTextAnchorPos;

	private void OnEnable()
	{
		m_mainButton = this.gameObject.GetComponent<Button>();
		for (int i = 0; i < this.gameObject.transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			var text = child.GetComponent<TextMeshProUGUI>();
			if (text != null)
			{
				m_textButton = text;
				m_defaultTextAnchorPos = m_textButton.rectTransform.anchoredPosition;
				break;
			}
		}

	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		m_isPointerDown = true;
		switch (m_mainButton.transition)
		{
			case Selectable.Transition.ColorTint:
				m_textButton.color = m_mainButton.colors.pressedColor;
				break;
			case Selectable.Transition.SpriteSwap:
				m_textButton.rectTransform.anchoredPosition = m_defaultTextAnchorPos + m_pressTextOffsetPos;
				break;
		}
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		m_isPointerDown = false;
		switch (m_mainButton.transition)
		{
			case Selectable.Transition.ColorTint:
				m_textButton.color = m_mainButton.colors.normalColor;
				break;
			case Selectable.Transition.SpriteSwap:
				m_textButton.rectTransform.anchoredPosition = m_defaultTextAnchorPos;
				break;
		}
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		switch (m_mainButton.transition)
		{
			case Selectable.Transition.ColorTint:
				m_textButton.color = m_mainButton.colors.highlightedColor;
				break;
			case Selectable.Transition.SpriteSwap:
				if (m_isPointerDown)
				{
					m_textButton.rectTransform.anchoredPosition = m_defaultTextAnchorPos + m_pressTextOffsetPos;
				}
				break;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		switch (m_mainButton.transition)
		{
			case Selectable.Transition.ColorTint:
				m_textButton.color = m_mainButton.colors.normalColor;
				break;
			case Selectable.Transition.SpriteSwap:
				if (m_isPointerDown)
				{
					m_textButton.rectTransform.anchoredPosition = m_defaultTextAnchorPos;
				}
				break;
		}
	}
}