using System;
using System.Collections.Generic;
using CrownAndAnchorGame;
using FluentAssertions;
using Xunit;

namespace UnitTestCrownAndAnchor
{
    public class RandomnessTest
    {
        // P(X=0) = (5/6 * 5/6 * 5/6) = 125/216 = 0.5787 
        //  0.5787  * 100 = 57.9
        [Fact]
        public void LossRandomness_IsApproximatelyConvergentToExpectation()
        {
            const int numThrows = 100000;

            const double tolerance = 30 / 100d;  // +- 30%
            const double above = 1 + tolerance;
            const double below = 1 - 1 / 100d;

            var dice = new Dice[] { new Dice(), new Dice(), new Dice() };


            foreach (DiceValue selectedFace in Enum.GetValues(typeof(DiceValue)))
            {
                //var selectedFace = DiceValue.CLUB;
                Console.WriteLine("Processing:" + selectedFace);

                double lossCount = 0;

                for (var i = 0; i <= numThrows; i++)
                {
                    var win = false;

                    foreach (var die in dice)
                    {
                        die.roll();
                        if (die.CurrentValue == selectedFace)
                            win = true;
                    }

                    if (!win)
                        lossCount++;
                }

                double houseBias = 125d / 216d - 1 / 2d;

                // Test house bias for losing should be 8% away from median. 
                lossCount.Should().BeInRange((58d/100d * numThrows) * below, (58d/100d * numThrows ) * above, "House bias for " + selectedFace + ", should be  :" + (houseBias * 100) + " numThrows/2:" + numThrows/2 + "  ");

                // win + loss ratio:
                var winRatio = (numThrows - lossCount) / numThrows;
                winRatio.Should().BeInRange(0.42 * below, 0.42 * above, "win + loss ratio should approximate 0.42");

            }

        }

        public void Win1Randomness_IsApproximatelyConvergentToExpectation()
        {
            const DiceValue selectedFace = DiceValue.SPADE;
            const int numDice = 3;
            const int numThrows = 10000;

            var hitCount = 0d;

            var die = new Dice();
            for (var i = 0; i <= numThrows; i++)
            {
                var hit = false;
                var rollCount = 0;
                for (var j = 0; j < numDice; j++)
                {
                    die.roll();
                    if (die.CurrentValue == selectedFace)
                        rollCount++;
                }
                if (rollCount == 1)
                    hitCount++;

            }

            hitCount.Should()
                .BeGreaterThan(34)
                .And.BeLessThan(35,
                    " P(X=1)=P (first dice match) +P(second dice match) +P(third dice match) =1/6 . 5/6 . 5/6 + 5/6 . 1/6 . 5/6 + 5/6 . 5/6 . 1/6 = 25/72 = 0.3472 And Should converge to:- 100 * 0.3472");
        }
    }
}
