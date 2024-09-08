namespace Vm167Box.Helpers.Tests;

[TestClass()]
public class SignalTests
{
    [DataTestMethod]
    [DataRow(0, 100, 50, 0)]
    [DataRow(10, 100, 50, 10)]
    [DataRow(50, 100, 50, 50)]
    [DataRow(100, 100, 50, 100)]
    [DataRow(25, 100, 25, 50)]
    [DataRow(50, 100, 25, 66.67)]
    [DataRow(99, 100, 99, 50)]
    public void DutyTest(double x, double period, int dutyCycle, double expected)
    {
        // Arrange
        // Act
        var result = Signal.Duty(x, period, dutyCycle);

        // Assert
        Assert.AreEqual(expected, result, 0.01);
    }
}