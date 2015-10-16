using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrownAndAnchorGame
{
    public class Game
    {
        private readonly List<IDice> dice;
        private readonly List<DiceValue> values;

        static List<DiceValue> dicelist = new List<DiceValue>();  // Todo: remove this.

        public IList<DiceValue> CurrentDiceValues
        {
            get { return values.AsReadOnly(); }
        }


        public Game(IDice die1, IDice die2, IDice die3)
        {
            dice = new List<IDice>();
            values = new List<DiceValue>();
            dice.Add(die1);
            dice.Add(die2);
            dice.Add(die3);

            foreach (var die in dice)
            {
                values.Add(die.CurrentValue);
            }
        }

        public int playRound(IPlayer player, DiceValue pick, int bet)
        {
            if (player == null) throw new ArgumentException("Player cannot be null");
            if (player == null) throw new ArgumentException("Pick cannot be null");
            if (bet < 0) throw new ArgumentException("Bet cannot be negative");

            player.takeBet(bet);

            int matches = 0;
            for (int i = 0; i < dice.Count; i++)
            {
                // Reroll the dice each time.
                dice[i].roll();

                values[i] = dice[i].CurrentValue;

                // I think the dice values are not being reset
                if (values[i].Equals(pick)) matches += 1;
                // matches = 3; Test 1
            }

            int winnings = matches * bet;
            if (matches > 0)
            {
                // Increase balance
                player.receiveWinnings(winnings);
            }

            return winnings;
        }
    }
}
