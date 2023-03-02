﻿using System;
using System.IO;
using System.Linq;

namespace OkayegTeaTime.Tools;

public sealed class SolutionCleaner
{
    private readonly string[] _dirsToClean =
    {
        "Build/",
        "OkayegTeaTime/bin/",
        "OkayegTeaTime.Database/bin/",
        "OkayegTeaTime.Models/bin/",
        "OkayegTeaTime.Settings/bin/",
        "OkayegTeaTime.Resources/bin/",
        "OkayegTeaTime.Spotify/bin/",
        "OkayegTeaTime.Tests/bin/",
        "OkayegTeaTime.Tools/bin/",
        "OkayegTeaTime.Twitch/bin/",
        "OkayegTeaTime.Utils/bin/",
        "TestResults/"
    };

    public void Clean()
    {
        foreach (string dir in _dirsToClean.Where(Directory.Exists))
        {
            try
            {
                Directory.Delete(dir, true);
                Console.WriteLine($"Deleted \"{dir}\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to delete \"{dir}\" => {ex.GetType().FullName}: {ex.Message}");
            }
        }
    }
}
