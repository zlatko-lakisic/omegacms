using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.Licensing
{
    public interface ILicensingSettings
    {
        ComponentEnum LicensingComponent { get; }
        string WorkingDirectory { get; set; }
        string ClientId { get; }
        string ClientKey { get; }
    }
}
