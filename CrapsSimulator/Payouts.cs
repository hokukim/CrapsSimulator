namespace CrapsSimulator
{
    public static class Payouts
    {
        public static double GetOddsPayout(int point)
        {
            if (point < 2)
            {
                // Invalid roll.
                return 0;
            }

            // Payout win.
            return (point switch
            {
                4 => 1.0 / 2.0,
                5 => 2.0 / 3.0,
                6 => 5.0 / 6.0,
                8 => 5.0 / 6.0,
                9 => 2.0 / 3.0,
                10 => 1.0 / 2.0,
                _ => throw new ArgumentException($"Invalid value: {point}", nameof(point))
            });
        }

        public static double GetPassLinePayout(double wager, BetResult betResult)
        {
            return betResult switch
            {
                BetResult.None => 0.0,
                BetResult.Win => wager * 2,
                BetResult.Lose => -wager,
                BetResult.Draw => 0.0,
                _ => throw new ArgumentException($"Invalid value: {betResult}", nameof(betResult))
            };
        }
    }
}
