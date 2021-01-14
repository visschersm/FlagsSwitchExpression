using System;
using System.Collections.Generic;

SomeEnum source = SomeEnum.Dog | SomeEnum.Value3;
SomeOtherEnum result = SomeOtherEnum.Unknown;

if (source.HasFlag(SomeEnum.Dog))
    result |= SomeOtherEnum.BigDog;

if (source.HasFlag(SomeEnum.Value2))
    result |= SomeOtherEnum.Value2;

if (source.HasFlag(SomeEnum.Value3))
    result |= SomeOtherEnum.Value3;

Console.WriteLine($"Result: {result}");


NonFlagEnum nonFlagResult = NonFlagEnum.Unknown;

nonFlagResult = source switch
{
    SomeEnum.Dog => NonFlagEnum.Value1,
    SomeEnum.Value2 => NonFlagEnum.Value2,
    SomeEnum.Value3 => NonFlagEnum.Value3,
    _ => NonFlagEnum.Unknown,
};

Console.WriteLine($"NonFlagResult: {nonFlagResult}");

var OtherTest = SomeOtherEnum.Unknown;

OtherTest = source switch
{
    var t when t.HasFlag(SomeEnum.Dog | SomeEnum.Value2 | SomeEnum.Value3) => SomeOtherEnum.BigDog | SomeOtherEnum.Value2 | SomeOtherEnum.Value3,
    var t when t.HasFlag(SomeEnum.Dog | SomeEnum.Value2) => SomeOtherEnum.BigDog | SomeOtherEnum.Value2,
    var t when t.HasFlag(SomeEnum.Dog | SomeEnum.Value3) => SomeOtherEnum.BigDog | SomeOtherEnum.Value3,
    var t when t.HasFlag(SomeEnum.Value2 | SomeEnum.Value3) => SomeOtherEnum.Value2 | SomeOtherEnum.Value3,
    SomeEnum.Dog => SomeOtherEnum.BigDog,
    SomeEnum.Value2 => SomeOtherEnum.Value2,
    SomeEnum.Value3 => SomeOtherEnum.Value3,
    SomeEnum.All => SomeOtherEnum.BigDog | SomeOtherEnum.Value2 | SomeOtherEnum.Value3,
    _ => SomeOtherEnum.Unknown,
};

Console.WriteLine($"OtherTest: {nonFlagResult}");

SomeOtherEnum myResult = SomeOtherEnum.Unknown;
foreach (SomeEnum type in GetFlags(source))
{
    myResult |= (type switch
    {
        SomeEnum.Dog => SomeOtherEnum.BigDog,
        SomeEnum.Value2 => SomeOtherEnum.Value2,
        SomeEnum.Value3 => SomeOtherEnum.Value3,
        SomeEnum.All => SomeOtherEnum.BigDog | SomeOtherEnum.Value2 | SomeOtherEnum.Value3,
        _ => SomeOtherEnum.Unknown
    });
}

Console.WriteLine($"SwitchExpressionResult: {myResult}");

IEnumerable<Enum> GetFlags(Enum input)
{
    foreach (Enum value in Enum.GetValues(input.GetType()))
        if (input.HasFlag(value))
            yield return value;
}


[Flags]
public enum SomeEnum
{
    Unknown = 0,
    Dog = 1,
    Value2 = 2,
    Value3 = 4,

    All = ~0
};

public enum NonFlagEnum
{
    Unknown = 0,
    Value1 = 1,
    Value2 = 2,
    Value3 = 3
};

[Flags]
public enum SomeOtherEnum
{
    Unknown = 0,
    BigDog = 1,
    Value2 = 2,
    Value3 = 4
};