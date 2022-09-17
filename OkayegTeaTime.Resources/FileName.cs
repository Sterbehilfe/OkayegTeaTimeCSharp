﻿using System;

namespace OkayegTeaTime.Resources;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FileName : Attribute
{
    public string Value { get; }

    public FileName(string value)
    {
        Value = value;
    }
}
