#pragma warning disable 618
using AssemblyToProcess;
using Fody;
using System;
using Xunit;

public class WeaverTests
{
    static TestResult testResult;

    static WeaverTests()
    {
        var weavingTask = new ModuleWeaver();
        testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");
    }

    [Fact]
    public void ValidateUpdatesUsingExplicitKeysOnStaticClass()
    {
        var type = testResult.Assembly.GetType(typeof(SampleStaticConfig).FullName);
        var expectedValue = Environment.GetEnvironmentVariable(SampleConfigTestValues.LOOKUP_KEY_A, EnvironmentVariableTarget.User);

        // Constant
        var constantResult = type.GetField(nameof(SampleStaticConfig.MyConstantKeyName)).GetRawConstantValue()?.ToString();
        Assert.Equal(expectedValue, constantResult);

        // Static
        var staticResult = type.GetField(nameof(SampleStaticConfig.MyStaticKeyName))?.GetValue(null);
        Assert.Equal(expectedValue, staticResult);

        // Static Readonly
        var staticReadonlyResult = type.GetField(nameof(SampleStaticConfig.MyStaticReadonlyKeyName))?.GetValue(null);
        Assert.Equal(expectedValue, staticReadonlyResult);
    }

    [Fact]
    public void ValidateUpdatesUsingExplicitKeys()
    {
        var type = testResult.Assembly.GetType(typeof(SampleConfig).FullName);
        var expectedValue = Environment.GetEnvironmentVariable(SampleConfigTestValues.LOOKUP_KEY_A, EnvironmentVariableTarget.User);
        var instance = (dynamic)Activator.CreateInstance(type);

        // Constant
        var constantResult = type.GetField(nameof(SampleConfig.MyConstantKeyName)).GetRawConstantValue()?.ToString();
        Assert.Equal(expectedValue, constantResult);

        // Private Static
        var privateStaticResult = type.GetMethod(nameof(SampleConfig.TestPrivateStaticWasSet))?.Invoke(null, null);
        Assert.Equal(expectedValue, privateStaticResult);

        // Static
        var staticResult = type.GetField(nameof(SampleConfig.MyStaticKeyName))?.GetValue(null);
        Assert.Equal(expectedValue, staticResult);

        // Static Readonly
        var staticReadonlyResult = type.GetField(nameof(SampleConfig.MyStaticReadonlyKeyName))?.GetValue(null);
        Assert.Equal(expectedValue, staticReadonlyResult);

        // Private Instance
        Assert.Equal(expectedValue, instance.TestPrivateWasSet());

        // Readonly Instance
        Assert.Equal(expectedValue, instance.MyReadonlyKeyName);

        // Public Instance
        Assert.Equal(expectedValue, instance.MyKeyName);
    }

    [Fact]
    public void ValidateUpdatesUsingImplicitKeys()
    {
        var type = testResult.Assembly.GetType(typeof(SampleImplicitConfig).FullName);
        var expectedValue = Environment.GetEnvironmentVariable(SampleConfigTestValues.LOOKUP_KEY_A, EnvironmentVariableTarget.User);
        var instance = (dynamic)Activator.CreateInstance(type);

        // Constant
        var constantResult = type.GetField(nameof(SampleImplicitConfig.MyConstantKeyName)).GetRawConstantValue()?.ToString();
        Assert.Equal(expectedValue, constantResult);

        // Public Instance
        Assert.Equal(expectedValue, instance.MyKeyName);
    }
}