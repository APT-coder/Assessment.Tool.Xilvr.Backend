namespace Assessment.Tool.Xilvr.Base.Helpers;

public static class Ensure
{
    //
    // Summary:
    //     Checks whether the string value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the int value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(int value, string message)
    {
        if (value <= 0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the nullable int value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(int? value, string message)
    {
        if (!value.HasValue || value <= 0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the long value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(long value, string message)
    {
        if (value <= 0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the nullable long value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(long? value, string message)
    {
        if (!value.HasValue || value <= 0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the short value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(short value, string message)
    {
        if (value <= 0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the nullable short value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(short? value, string message)
    {
        if (!value.HasValue || value <= 0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the double value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(double value, string message)
    {
        if (value <= 0.0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the nullable double value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(double? value, string message)
    {
        if (!value.HasValue || value <= 0.0)
        {
            throw new ArgumentNullException(message);
        }
    }

    //
    // Summary:
    //     Checks whether the object value is valid or not
    //
    // Parameters:
    //   value:
    //
    //   message:
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    public static void IsNotNull(object value, string message)
    {
        if (value == null)
        {
            throw new ArgumentNullException(message);
        }
    }
}
