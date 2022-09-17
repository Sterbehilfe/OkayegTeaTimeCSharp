﻿using System;

namespace OkayegTeaTime.Twitch.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public sealed class TimePattern : Attribute
{
    public string ConversionMethod { get; }

    public int Factor { get; }

    public TimePattern(string conversionMethod, int factor = 1)
    {
        ConversionMethod = conversionMethod;
        Factor = factor;
    }
}
