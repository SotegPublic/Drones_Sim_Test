using System.Collections.Generic;
using Zenject;

public class FractionsHolder : IFractionsHolder
{
    private Fraction[] _fractions;

    [Inject]
    public FractionsHolder(Fraction[] fractions, GameConfig gameConfig)
    {
        _fractions = fractions;

        for(int i = 0; i < _fractions.Length; i++)
        {
            _fractions[i].DronesPriorities = new HashSet<int>(gameConfig.MaxDronesCount);
        }
    }

    public Fraction[] Fractions => _fractions;
}
