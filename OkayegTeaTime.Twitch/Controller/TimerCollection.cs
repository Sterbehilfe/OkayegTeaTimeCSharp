﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace OkayegTeaTime.Twitch.Controller;

public sealed class TimerCollection : IEnumerable<Timer>
{
    private readonly List<Timer> _timers = new();

    public void Add(Action<object?, ElapsedEventArgs> action, double interval, bool autoReset = true)
    {
        Timer? timer = _timers.FirstOrDefault(t => Math.Abs(t.Interval - interval) <= 0 && t.AutoReset == autoReset);
        if (timer is null)
        {
            timer = new(interval)
            {
                AutoReset = autoReset
            };
            _timers.Add(timer);
        }

        timer.Elapsed += (sender, e) => action(sender, e);
        if (!timer.Enabled)
        {
            timer.Start();
        }
    }

    public IEnumerator<Timer> GetEnumerator()
    {
        return _timers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
