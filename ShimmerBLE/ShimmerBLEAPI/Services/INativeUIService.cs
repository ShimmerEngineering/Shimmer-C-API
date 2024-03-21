using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmerBLEAPI.Services
{
    public interface INativeUIService
    {
        void Invoke(Action action);
    }
}
