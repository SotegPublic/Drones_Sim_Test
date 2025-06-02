using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainUIView : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text _firstFractionName;
    [SerializeField] private TMP_Text _secondFractionName;
    [SerializeField] private TMP_Text _firstFractionScore;
    [SerializeField] private TMP_Text _secondFractionScore;

    [Header("Control")]
    [SerializeField] private PointerUpSlider _dronesCountSlider;
    [SerializeField] private PointerUpSlider _dronesSpeedSlider;
    [SerializeField] private TMP_InputField _resourceGenerateSpeed;
    [SerializeField] private Toggle _showPathToggle;

    public TMP_Text FirstFractionName => _firstFractionName;
    public TMP_Text SecondFractionName => _secondFractionName;
    public TMP_Text FirstFractionScore => _firstFractionScore;
    public TMP_Text SecondFractionScore => _secondFractionScore;
    public PointerUpSlider DronesCountSlider => _dronesCountSlider;
    public PointerUpSlider DronesSpeedSlider => _dronesSpeedSlider;
    public TMP_InputField ResourceGenerateSpeed => _resourceGenerateSpeed;
    public Toggle ShowPathToggle => _showPathToggle;

    [Inject]
    public void InitView(IFractionsHolder fractionsHolder, GameConfig gameConfig)
    {
        InitScore(fractionsHolder);
        InitCotrol(gameConfig);
    }

    private void InitCotrol(GameConfig gameConfig)
    {
        _dronesCountSlider.maxValue = gameConfig.MaxDronesCount;
        _dronesCountSlider.minValue = 1;
        _dronesCountSlider.value = gameConfig.StartDronesCount;

        _dronesSpeedSlider.maxValue = gameConfig.MaxDronesSpeed;
        _dronesSpeedSlider.minValue = 1;
        _dronesSpeedSlider.value = gameConfig.StartDronesSpeed;

        _resourceGenerateSpeed.text = Math.Round(gameConfig.SpawnResourcesFrequency, 1).ToString();
        _showPathToggle.isOn = false;
    }

    private void InitScore(IFractionsHolder fractionsHolder)
    {
        _firstFractionName.text = fractionsHolder.Fractions[0].FractionName;
        _secondFractionName.text = fractionsHolder.Fractions[1].FractionName;

        _firstFractionScore.text = "0";
        _secondFractionScore.text = "0";
    }
}
