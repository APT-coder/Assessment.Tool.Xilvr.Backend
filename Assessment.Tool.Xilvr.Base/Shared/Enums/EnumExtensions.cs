using System.ComponentModel;
using System.Reflection;

namespace Assessment.Tool.Xilvr.Base.Shared.Enums;

//
// Summary:
//     Extensions for Enums
public static class EnumExtensions
{
    //
    // Summary:
    //     Get enum from given value.
    //
    // Parameters:
    //   value:
    //
    // Type parameters:
    //   TEnum:
    //
    // Exceptions:
    //   T:System.ArgumentException:
    public static TEnum GetEnumFromValue<TEnum>(int value) where TEnum : Enum
    {
        if (Enum.IsDefined(typeof(TEnum), value))
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), value);
        }

        throw new ArgumentException($"Enum value {value} is not defined in {typeof(TEnum).Name}");
    }

    //
    // Summary:
    //     Get enum from given name.
    //
    // Parameters:
    //   name:
    //
    // Type parameters:
    //   TEnum:
    //
    // Exceptions:
    //   T:System.ArgumentException:
    public static TEnum GetEnumFromName<TEnum>(string name) where TEnum : Enum
    {
        if (Enum.IsDefined(typeof(TEnum), name))
        {
            return (TEnum)Enum.Parse(typeof(TEnum), name);
        }

        throw new ArgumentException("Enum name '" + name + "' is not defined in " + typeof(TEnum).Name);
    }

    //
    // Summary:
    //     Check if the given value is defined in enum.
    //
    // Parameters:
    //   value:
    //
    // Type parameters:
    //   TEnum:
    public static bool IsValueDefined<TEnum>(int value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }

    //
    // Summary:
    //     Retrieves the description attribute value of an enumvalue using reflection.
    //
    // Parameters:
    //   value:
    //     The enumeration value.
    public static string ToDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        if (field != null)
        {
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
        }

        return value.ToString();
    }

    //
    // Summary:
    //     Retrieves the enum value associated with a given description attribute value.
    //
    //
    // Parameters:
    //   description:
    //     The description attribute value to match.
    //
    // Type parameters:
    //   T:
    //     The enumeration type.
    //
    // Returns:
    //     The enumeration value.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     Thrown if no enum value with the specified description is found.
    public static T GetEnumValueFromDescription<T>(string description) where T : Enum
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                if (descriptionAttribute != null && descriptionAttribute.Description == description)
                {
                    return value;
                }
            }
        }

        throw new ArgumentException("No enum value with description '" + description + "' found.");
    }
}
