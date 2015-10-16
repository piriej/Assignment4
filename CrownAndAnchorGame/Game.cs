using System;
using System.Collections.Generic;

namespace CrownAndAnchorGame
{
    public class Game
    {
        private readonly List<IDice> _dice;
        private readonly List<DiceValue> _values;

        public IList<DiceValue> CurrentDiceValues => _values.AsReadOnly();

        public Game(IDice die1, IDice die2, IDice die3)
        {
            _dice = new List<IDice>();
            _values = new List<DiceValue>();
            _dice.Add(die1);
            _dice.Add(die2);
            _dice.Add(die3);

            foreach (var die in _dice)
            {
                _values.Add(die.CurrentValue);
            }
        }

        public int PlayRound(IPlayer player, DiceValue pick, int bet)
        {
            if (player == null) throw new ArgumentException("Player cannot be null");
            if (player == null) throw new ArgumentException("Pick cannot be null");
            if (bet < 0) throw new ArgumentException("Bet cannot be negative");

            // Deduct the bet from the player.
            player.takeBet(bet);

            var matches = 0;
            for (var i = 0; i < _dice.Count; i++)
            {
                // Roll each dice.
                _dice[i].roll();

                // Set the current dice values.
                _values[i] = _dice[i].CurrentValue;

                // Reset the dice values.
                if (_values[i].Equals(pick)) matches += 1;
            }

            // Check if there were no winnings.
            if (matches <= 0) return 0;

            // Calculate the winnings.
            var winnings = matches * bet + bet;

            // Increase balance
            player.receiveWinnings(winnings);
            return winnings;
        }
    }
}
