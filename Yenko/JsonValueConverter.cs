using Cocona.Command.Binder;

namespace Yenko;

/// <summary>
/// Helper class to convert JSON argument to complex type.
/// </summary>
public class JsonValueConverter : ICoconaValueConverter
{
    public object? ConvertTo(Type t, string? value)
    {
        if (value is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize(value, t);
    }
}