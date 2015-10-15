using System;
using System.Collections.Generic;
using System.Linq;
using CrownAndAnchorGame;
using FluentAssertions;
using Xunit.Extensions;
using Xunit.Sdk;

namespace UnitTestCrownAndAnchor
{
    public class WinningsTest
    {
        [Theory, AutoNSubstituteData]
        public void Ensure(/*Dice die1, Dice die2, Dice die3*/)
        {
            var die1 = new Dice();
            var die2 = new Dice();
            var die3 = new Dice();

            // Introduce a player with $100
            var player = new Player("SomeGuy", 100);

            // Choose a symbol from the playmat.
            var pick = Dice.RandomValue;
           
            // Place a bet of $5
            const int bet = 5;

            // Start a game
            var game = new Game(die1, die2, die3);
            var numMatches = 0;
            var numturns = 0;

            while (player.balanceExceedsLimitBy(bet) && player.Balance < 150)
            {
                numturns++;
                IList<DiceValue> currentValues1 = new List<DiceValue>(game.CurrentDiceValues);
                game.playRound(player, pick, bet);
                var currentValues2 = game.CurrentDiceValues;

                try
                {
                    currentValues1.Should().NotBeEquivalentTo(currentValues2);
                }
                catch (AssertException e)
                {
                    numMatches++;
                }
            }

            numMatches.Should().BeLessThan((int)Math.Round(0.125 * numturns));
        } 


    [Theory, AutoNSubstituteData]
    public void TestGamePlayRound(Dice die1, Dice die2, Dice die3, Player player)
    {
        // Setup initial conditions.
        const int bet = 5;
        player.Limit = 0;
        var winnings = 0;
        var pick = Dice.RandomValue;

        int totalWins = 0;
        int totalLosses = 0;

        var game = new Game(die1, die2, die3);

        winnings = game.playRound(player, die1.CurrentValue, bet);
        winnings.Should().Be(bet);
    }

    [Theory, AutoNSubstituteData]
    public void TestGameDiceRandomness()
    {
        var dice = new Dice();
        var values = new List<DiceValue>();

        for (var i = 0; i < 100; i++)
        {
            values.Add(dice.roll());
        }

        values.Distinct().Count().Should().Be(5);
    }



    //[Theory, AutoNSubstituteData]
    //public void Teststuff(Dice die1, Dice die2, Dice die3, Player player, int bet)
    //{
    //    //player.Limit = 0;
    //    //balance - bet > limit

    //    player.takeBet().Returns(true);

    //    var game = new Game(die1, die2, die3);

    //    var winnings = game.playRound(player, (DiceValue)die1.CurrentValue, bet);

    //    winnings.Should().Be(bet);
    //}

    //private Player PlayerFixture()
    //{

    //}
}
}
