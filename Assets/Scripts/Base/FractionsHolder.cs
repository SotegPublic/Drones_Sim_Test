using Zenject;

public class FractionsHolder : IFractionsHolder
{
    private Fraction[] _fractions;

    [Inject]
    public FractionsHolder(Fraction[] fractions)
    {
        _fractions = fractions;
    }

    public Fraction[] Fractions => _fractions;
}
