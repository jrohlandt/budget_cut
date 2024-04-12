using System;
using Godot;
using Newtonsoft.Json;

namespace BudgetApp;

public static class Logger
{

    public static void Log(Object data)
    {
        // write to AppGlobals.LogPath
        // todo 
        
        if (AppGlobals.Debug) {
            GD.Print("Debug Log", JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
