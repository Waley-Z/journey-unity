using System;
using System.Collections.Generic;

public static class Events
{
    public static CameraButtonPressedEvent CameraButtonPressedEvent = new();
}

public class CameraButtonPressedEvent : GameEvent { }
