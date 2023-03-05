namespace CrapsSimulator.Strategies
{
    internal interface IStrategy
    {
        double Bankroll { get; }

        Task ResolveComeOutWin();
        Task ResolveComeOutCraps(bool is12);
        Task ResolvePointMatch(int point);
        Task ResolvePointCraps(int point);

    }
}
