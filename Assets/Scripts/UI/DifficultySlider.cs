using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class DifficultySlider : MonoBehaviour {

	private DifficultySetting[] settings;

	private Slider sliderComponent;
	private Image sliderImage;
	private Image handleImage;

	public Text difficultyText;

	void Awake ()
	{
		settings = GameManager.instance.GetDifficultySettings();
		sliderComponent = GetComponent<Slider>();

		sliderImage = sliderComponent.fillRect.GetComponent<Image>();
		handleImage = sliderComponent.handleRect.GetComponent<Image>();
		
		sliderComponent.minValue = 0;
		sliderComponent.maxValue = settings.Length - 1;
		sliderComponent.value = 0;
		OnChanged (0.0f);
	}
	
	void UpdateDifficultyText (float value) 
	{
		difficultyText.text = settings[(int)value].displayText;
	}

	void UpdateDifficultyColor (float value)
	{
		Color startColor = Color.green;
		Color midpointColor = Color.yellow;
		Color endColor = Color.red;

		Color selectedColor;

		float midpoint = sliderComponent.maxValue / 2.0f;

		if (value < midpoint)
		{
			selectedColor = Color.Lerp(startColor, midpointColor, value / midpoint);
		}
		else 
		{
			selectedColor = Color.Lerp (midpointColor, endColor, (value - midpoint) / (sliderComponent.maxValue - midpoint));
		}

		sliderImage.color = selectedColor;
		handleImage.color = selectedColor;
	}

	void OnChanged(float value)
	{
		UpdateDifficultyText(value);
		UpdateDifficultyColor(value);
	}

	void OnEnable ()
	{
		sliderComponent.onValueChanged.AddListener(OnChanged);
	}

	void OnDisable ()
	{
		sliderComponent.onValueChanged.RemoveListener(OnChanged);
	}

}
