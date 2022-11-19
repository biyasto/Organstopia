using System;
using Unity.Services.CloudCode;

namespace Unity.Services.Samples.BattlePass
{
    public class CloudCodeResultUnavailableException : Exception
    {
        public CloudCodeException cloudCodeException { get; private set; }

        public CloudCodeResultUnavailableException(CloudCodeException cloudCodeException, string message = null) : base(message)
        {
            this.cloudCodeException = cloudCodeException;
        }
    }
}
