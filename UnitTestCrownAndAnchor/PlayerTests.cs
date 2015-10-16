using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrownAndAnchorGame;
using FluentAssertions;
using Xunit.Extensions;

namespace UnitTestCrownAndAnchor
{
    public class PlayerTests
    {
        [Theory, AutoNSubstituteData]
        public void BalanceExceedsLimitBy_With5DollarBetWhenOnly5DollarsRemaining_ReturnsTrue(string name)
        {
            var player = new Player(name, 5) {Limit = 0};

            player.balanceExceedsLimitBy(5).Should().BeTrue();
        }


        [Theory, AutoNSubstituteData]
        public void BalanceExceedsLimitBy_With5DollarBetWhenOnly4DollarsRemaining_ReturnsFalse(string name)
        {
            var player = new Player(name, 4) { Limit = 0 };

            player.balanceExceedsLimitBy(5).Should().BeFalse();
        }

        [Theory, AutoNSubstituteData]
        public void BalanceExceedsLimitBy_With5DollarBetWhenOnly6DollarsRemaining_ReturnsFalse(string name)
        {
            var player = new Player(name, 6) { Limit = 0 };

            player.balanceExceedsLimitBy(5).Should().BeTrue();
        }
    }
}
