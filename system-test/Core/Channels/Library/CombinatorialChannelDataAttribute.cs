using System.Reflection;
using Xunit.Sdk;

namespace Optivem.EShop.SystemTest.Core.Channels.Library;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CombinatorialChannelDataAttribute : DataAttribute
{
    private readonly string[] _channels;

    public CombinatorialChannelDataAttribute(params string[] channels)
    {
        _channels = channels ?? throw new ArgumentNullException(nameof(channels));

        if (_channels.Length == 0)
            throw new ArgumentException("At least one channel must be specified", nameof(channels));
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Get all CombinatorialDataRow attributes
        var dataRowAttributes = testMethod
            .GetCustomAttributes(typeof(CombinatorialDataRowAttribute), false)
            .Cast<CombinatorialDataRowAttribute>()
            .ToArray();

        if (dataRowAttributes.Length == 0)
        {
            throw new InvalidOperationException(
                $"Method {testMethod.Name} must have at least one [CombinatorialDataRow] attribute");
        }

        // Create Cartesian product: channels × data rows
        foreach (var channel in _channels)
        {
            foreach (var dataRowAttr in dataRowAttributes)
            {
                var testCase = new List<object> { new Channel(channel) };
                testCase.AddRange(dataRowAttr.Data);
                yield return testCase.ToArray();
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CombinatorialDataRowAttribute : Attribute
{
    public object[] Data { get; }

    /// <summary>
    /// Specifies test data parameters (excluding the channel parameter).
    /// </summary>
    /// <param name="data">Test data values</param>
    public CombinatorialDataRowAttribute(params object[] data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}

