using System;
using Xunit;
using BPCalculator;

namespace UnitTestBloodPressure
{
    public class BloodPressureCategoryTests
    {
        public BloodPressure BP;

        [Theory]
        [InlineData(119,79)]
        [InlineData(111,75)]
        public void Test_for_normal_values(int s, int d)
        {
            BP = new BloodPressure() { Systolic = s, Diastolic = d };
            Assert.Equal(BPCalculator.BPCategory.Normal, BP.Category);
        }

        [Theory]
        [InlineData(124,79)]
        public void Test_for_elevated_values(int s, int d)
        {
            BP = new BloodPressure() { Systolic = s, Diastolic = d };
            Assert.Equal(BPCalculator.BPCategory.Elevated, BP.Category);
        }

        [Theory]
        [InlineData(181, 79)]
        [InlineData(181, 80)]
        [InlineData(181, 85)]
        [InlineData(181, 95)]
        [InlineData(130, 121)]
        [InlineData(141, 121)]
        [InlineData(181, 121)]
        public void Test_for_crisis_values(int s, int d)
        {
            BP = new BloodPressure() { Systolic = s, Diastolic = d };
            Assert.Equal(BPCalculator.BPCategory.Crisis, BP.Category);
        }

        [Theory]
        [InlineData(130, 79)]
        [InlineData(130, 80)]
        [InlineData(139, 85)]
        [InlineData(139, 89)]
        public void Test_for_high1_values(int s, int d)
        {
            BP = new BloodPressure() { Systolic = s, Diastolic = d };
            Assert.Equal(BPCalculator.BPCategory.High1, BP.Category);
        }

        [Theory]
        [InlineData(140, 79)]
        [InlineData(150, 79)]
        [InlineData(180, 79)]
        [InlineData(140, 90)]
        [InlineData(140, 98)]
        public void Test_for_high2_values(int s, int d)
        {
            BP = new BloodPressure() { Systolic = s, Diastolic = d };
            Assert.InRange(d, BloodPressure.DiastolicMin, BloodPressure.DiastolicMax);
            Assert.Equal(BPCalculator.BPCategory.High2, BP.Category);
        }

        [Fact]
        public void Test_for_validata()
        {
            int s = 130;
            int d = 150;
            BP = new BloodPressure() { Systolic = s, Diastolic = d };
            Assert.Throws<System.Exception>(() => BP.Category);            
        }
    }
}
