using System;
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
        _view.ResourceGenerateSpeed.onSubmit.AddListener(WhenGenerationSpeedChange);
        _view.ResourceGenerateSpeed.onEndEdit.AddListener(WhenGenerationSpeedChange);
        _view.ShowPathToggle.onValueChanged.AddListener(WhenPathToggleChange);

        _fractionsHolder.Fractions[0].OnGetResource += UpdateFirstFractionScore;
        _fractionsHolder.Fractions[1].OnGetResource += UpdateSecondFractionScore;
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
        if(float.TryParse(speedStr, out var speed))
        {
            OnGenerationSpeedChange?.Invoke(speed);
        }
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
        _view.ResourceGenerateSpeed.onSubmit.RemoveListener(WhenGenerationSpeedChange);
        _view.ShowPathToggle.onValueChanged.RemoveListener(WhenPathToggleChange);

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
