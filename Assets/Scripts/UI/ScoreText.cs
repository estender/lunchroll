using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour {

	void OnEnable()
	{
		GameEventManager.OnItemCollected += UpdateText;
	}

	void OnDisable()
	{
		GameEventManager.OnItemCollected -= UpdateText;
	}

	void UpdateText(ItemCollectedArgs args)
	{
		Text textComponent = GetComponent<Text>();
		textComponent.text = args.Count.ToString();
	}
}
