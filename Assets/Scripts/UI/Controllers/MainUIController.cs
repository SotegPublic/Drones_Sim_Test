using System;
using TMPro;
using UnityEngine;
using Zenject;

public class MainUIController : IInitializable, IDisposable, IMainUINotifier
{
    public Action<bool> OnPathToggleChange { get; set; }
    public Action<int> OnDronesCountChange { get; set; }
    public Action<float> OnDroneSpeedChange { get; set; }
    public Action<float> OnGenerationSpeedChange { get; set; }

    private MainUIView _view;
    private IFractionsHolder _fractionsHolder;

    [Inject]
    public MainUIController(MainUIView view, IFractionsHolder fractionsHolder)
    {
        _view = view;
        _fractionsHolder = fractionsHolder;
    }

    [Inject]
    public void Initialize()
    {
        _view.DronesCountSlider.OnSliderPointerUp += WhenDroneCountChange;
        _view.DronesSpeedSlider.OnSliderPointerUp += WhenDroneSpeedChange;
        _view.DronesCountSlider.onValueChanged.AddListener(UpdateCountSliderText);
        _view.DronesSpeedSlider.onValueChanged.AddListener(UpdateSpeedSliderText);

        _view.ResourceGenerateSpeed.onEndEdit.AddListener(WhenGenerationSpeedChange);
        _view.ShowPathToggle.onValueChanged.AddListener(WhenPathToggleChange);
        _view.ExitButton.onClick.AddListener(Exit);

        _fractionsHolder.Fractions[0].OnGetResource += UpdateFirstFractionScore;
        _fractionsHolder.Fractions[1].OnGetResource += UpdateSecondFractionScore;
    }

    private void UpdateSpeedSliderText(float value)
    {
        _view.DronesSpeedSliderText.text = Math.Round(value,1).ToString();
    }

    private void UpdateCountSliderText(float value)
    {
        _view.DronesCountSliderText.text = value.ToString();
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void UpdateSecondFractionScore(int score)
    {
        _view.SecondFractionScore.text = score.ToString();
    }

    private void UpdateFirstFractionScore(int score)
    {
        _view.FirstFractionScore.text = score.ToString();
    }

    private void WhenPathToggleChange(bool isOn)
    {
        OnPathToggleChange?.Invoke(isOn);
    }

    private void WhenGenerationSpeedChange(string speedStr)
    {
        if (float.TryParse(speedStr, out var speed))
        {
            var roundedSpeed = (float)Math.Round(speed,1);

            _view.ResourceGenerateSpeed.text = roundedSpeed.ToString();
            PlaceCaret(_view.ResourceGenerateSpeed);
            
            OnGenerationSpeedChange?.Invoke(roundedSpeed);
        }
    }

    private void PlaceCaret(TMP_InputField inputField)
    {
        var textRect = inputField.textComponent.GetComponent<RectTransform>();
        var caret = inputField.GetComponentInChildren<TMP_SelectionCaret>();

        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        if (caret != null)
        {
            var caretRect = caret.GetComponent<RectTransform>();
            caretRect.offsetMin = Vector2.zero;
            caretRect.offsetMax = Vector2.zero;
        }

        inputField.caretPosition = 0;
        inputField.ForceLabelUpdate();
    }

    private void WhenDroneSpeedChange(float speed)
    {
        OnDroneSpeedChange?.Invoke(speed);
    }

    private void WhenDroneCountChange(float count)
    {
        OnDronesCountChange?.Invoke((int)count);
    }

    public void Dispose() 
    {
        _view.DronesCountSlider.OnSliderPointerUp -= WhenDroneCountChange;
        _view.DronesSpeedSlider.OnSliderPointerUp -= WhenDroneSpeedChange;
        _view.DronesCountSlider.onValueChanged.RemoveListener(UpdateCountSliderText);
        _view.DronesCountSlider.onValueChanged.RemoveListener(UpdateSpeedSliderText);
        _view.ResourceGenerateSpeed.onSubmit.RemoveListener(WhenGenerationSpeedChange);
        _view.ShowPathToggle.onValueChanged.RemoveListener(WhenPathToggleChange);
        _view.ExitButton.onClick.RemoveListener(Exit);

        _fractionsHolder.Fractions[0].OnGetResource -= UpdateFirstFractionScore;
        _fractionsHolder.Fractions[1].OnGetResource -= UpdateSecondFractionScore;
    }
}
public interface IMainUINotifier
{
    public Action<bool> OnPathToggleChange { get; set; }
    public Action<int> OnDronesCountChange { get; set; }
    public Action<float> OnDroneSpeedChange { get; set; }
    public Action<float> OnGenerationSpeedChange { get; set; }
}
