using System.Net.Http.Headers;

namespace CrapsSimulator.Strategies
{
    internal class NoPassLayOddsStrategy : IStrategy
    {
        private const double PASSLINE_WAGER = 5.0;
        private const double ODDS_WAGER = 12.0;

        public double Bankroll { get; private set; }

        public NoPassLayOddsStrategy()
        {
            //AddLog($"Game start: {nameof(NoPassLayOddsStrategy)}");
        }

        /// <summary>
        /// This strategy loses when the ComeOut roll wins.
        /// </summary>
        public Task ResolveComeOutWin()
        {
            Bankroll -= PASSLINE_WAGER;

            return Task.CompletedTask;
        }

        /// <summary>
        /// This strategy wins when the ComeOut roll craps.
        /// Push on 12.
        /// </summary>
        public Task ResolveComeOutCraps(bool is12)
        {
            if (is12)
            {
                return Task.CompletedTask;
            }

            Bankroll += PASSLINE_WAGER;

            return Task.CompletedTask;
        }

        /// <summary>
        /// This strategy loses when the Point is matched.
        /// </summary>
        public Task ResolvePointMatch(int point)
        {
            Bankroll -= PASSLINE_WAGER;
            Bankroll -= ODDS_WAGER;

            return Task.CompletedTask;
        }

        /// <summary>
        /// This strategy wins when the Point roll craps.
        /// </summary>
        /// <returns></returns>
        public Task ResolvePointCraps(int point)
        {
            Bankroll += PASSLINE_WAGER;
            Bankroll += ODDS_WAGER * Payouts.GetOddsPayout(point);

            return Task.CompletedTask;
        }
    }
}
