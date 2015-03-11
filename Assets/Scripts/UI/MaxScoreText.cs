using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class MaxScoreText : MonoBehaviour {

	/// <summary>
	/// Register OnMapGenerated listener on enable
	/// </summary>
	void OnEnable()
	{
		GameEventManager.OnMapGenerated += UpdateText;
	}

	/// <summary>
	/// Remove OnMapGenerated listener on disable
	/// </summary>
	void OnDisable()
	{
		GameEventManager.OnMapGenerated -= UpdateText;
	}
	
	/// <summary>
	/// Updates the max score after map generation
	/// </summary>
	/// <param name="args">Map Generated Arguments.</param>
	void UpdateText(MapGeneratedArgs args)
	{
		Text textComponent = GetComponent<Text>();
		textComponent.text = args.TreasureCount.ToString();
	}
}
