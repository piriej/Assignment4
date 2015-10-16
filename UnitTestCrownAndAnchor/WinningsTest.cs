using System;
using System.Collections.Generic;
using System.Linq;
using CrownAndAnchorGame;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

namespace UnitTestCrownAndAnchor
{
    public class WinningsTest
    {
        public static IEnumerable<object[]> Lose
        {
            get { yield return new object[] { DiceValue.CLUB, DiceValue.ANCHOR, DiceValue.ANCHOR, DiceValue.ANCHOR, 100, 5, 95 }; }
        }

        public static IEnumerable<object[]> Win1Correct
        {
            get { yield return new object[] { DiceValue.CLUB, DiceValue.CLUB, DiceValue.ANCHOR, DiceValue.ANCHOR, 100, 5, 5, 110 }; }
        }

        public static IEnumerable<object[]> Win2Correct
        {
            get { yield return new object[] { DiceValue.CLUB, DiceValue.CLUB, DiceValue.ANCHOR, DiceValue.CLUB, 100, 5, 10, 115 }; }
        }

        public static IEnumerable<object[]> Win3Correct
        {
            get { yield return new object[] { DiceValue.CLUB, DiceValue.CLUB, DiceValue.CLUB, DiceValue.CLUB, 100, 5, 15, 120 }; }
        }

        [Theory, AutoNSubstituteData]
        [AutoNSubstitutePropertyData("Lose")]
        [AutoNSubstitutePropertyData("Win1Correct")]
        [AutoNSubstitutePropertyData("Win2Correct")]
        [AutoNSubstitutePropertyData("Win3Correct")]
        public void GivenRoundIsPlayed_WhenTheplayersPickMatchesNoDie_5DollarsIsDeductedFromTheBalance(
            DiceValue pick,
            DiceValue dieValue1,
            DiceValue dieValue2,
            DiceValue dieValue3,
            int balance,
            int bet,
            int winnings,
            int total,
            IDice die1, 
            IDice die2, 
            IDice die3,
            IPlayer player)
        {
            // Arrange.
            // Set the value of the dice rolled.
            die1.CurrentValue.Returns(dieValue1);
            die2.CurrentValue.Returns(dieValue2);
            die3.CurrentValue.Returns(dieValue3);

            die1.roll().Returns(dieValue1);
            die2.roll().Returns(dieValue2);
            die3.roll().Returns(dieValue3);

            // Taking a bet deducts money from the player.
            player.When(x => x.takeBet(Arg.Any<int>()));//.Do(x => total -= bet);

            var game = new Game(die1, die2, die3);

            // Act
            var sut = game.playRound(player, pick, bet);

            // Assert
            sut.Should().Be(winnings);
        }


        [Theory, AutoNSubstituteData]
        public void ShouldIncrease(/*Dice die1, Dice die2, Dice die3*/)
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
