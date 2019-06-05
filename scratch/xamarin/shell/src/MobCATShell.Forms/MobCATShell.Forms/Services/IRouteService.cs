using System;
using System.Collections.Generic;

namespace MobCATShell.Forms.Services
{
    public interface IRouteService
    {
        string NestedARoute { get; }

        string NestedBRoute { get; }

        string ShellDetailsRoute { get; }

        string ADetailsRoute { get; }

        string BDetailsRoute { get; }

        string BasicNavTabRoute { get; }

        string GetRandomRoute();
    }
}
