﻿using System.Reflection;
using System.Text.RegularExpressions;

namespace OkayegTeaTime.Twitch.Models;

public sealed class TimeConversionMethod
{
    public Regex Regex { get; }

    public MethodInfo Method { get; }

    public int Factor { get; }

    public TimeConversionMethod(Regex regex, MethodInfo method, int factor)
    {
        Regex = regex;
        Method = method;
        Factor = factor;
    }
}
