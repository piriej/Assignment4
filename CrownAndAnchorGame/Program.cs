using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrownAndAnchorGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Dice.
            IDice d1 = new Dice();
            IDice d2 = new Dice();
            IDice d3 = new Dice();

            // Create player.
            IPlayer p = new Player("Fred", 100);
            Console.WriteLine(p);
            Console.WriteLine();

            // Create Game.
            Console.WriteLine("New game for {0}", p.Name);
            Game g = new Game(d1, d2, d3);

            // Initial dice values? Why does this matter?
            IList<DiceValue> cdv = g.CurrentDiceValues;
            Console.WriteLine("Current dice values : {0} {1} {2}", cdv[0], cdv[1], cdv[2]);

            // Unused random dice value? Why is this done?
            DiceValue rv = Dice.RandomValue;

            // Unused random
            Random random = new Random();

            // Setup initial conditions.
            int bet = 5;
            p.Limit = 0;
            int winnings = 0;
            DiceValue pick = Dice.RandomValue;
            Console.WriteLine("Chosen Dice Value:" + pick);

            int totalWins = 0;
            int totalLosses = 0;

            while (true)
            {
                int winCount = 0;
                int loseCount = 0;

                // Play 100 times
                for (int i = 0; i < 100; i++)
                {
                    // Re-creating player with the same balance each time.
                    p = new Player("Fred", 100);
                    Console.Write("Start Game {0}: ", i);
                    Console.WriteLine("{0} starts with balance {1}", p.Name, p.Balance);
                    int turn = 0;

                    // Balance lower limit 0 upper limit 200.
                    while (p.balanceExceedsLimitBy(bet) && p.Balance < 200)
                    {
                        try
                        {
                            winnings = g.playRound(p, pick, bet);
                            cdv = g.CurrentDiceValues;

                            Console.WriteLine("Rolled {0} {1} {2}", cdv[0], cdv[1], cdv[2]);
                            if (winnings > 0)
                            {
                                Console.WriteLine("{0} won {1} balance now {2}", p.Name, winnings, p.Balance);
                                winCount++;
                            }
                            else
                            {
                                Console.WriteLine("{0} lost {1} balance now {2}", p.Name, bet, p.Balance);
                                loseCount++;
                            }
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("{0}\n\n", e.Message);
                        }
                        pick = Dice.RandomValue;
                        winnings = 0;
                        turn++;
                    } //while

                    Console.Write("{1} turns later.\nEnd Game {0}: ", turn, i);
                    Console.WriteLine("{0} now has balance {1}\n", p.Name, p.Balance);
                } //for

                Console.WriteLine("Win count = {0}, Lose Count = {1}, {2:0.00}", winCount, loseCount, (float)winCount / (winCount + loseCount));
                totalWins += winCount;
                totalLosses += loseCount;

                string ans = Console.ReadLine();
                if (ans.Equals("q")) break;
            } //while true
            Console.WriteLine("Overall win rate = {0}%", (float)(totalWins * 100) / (totalWins + totalLosses));
            Console.ReadLine();
        }
    }
}
