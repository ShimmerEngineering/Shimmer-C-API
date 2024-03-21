using System;
using System.Collections.Generic;
using System.Text;

namespace shimmer.Services
{
    public interface IToast
    {
        void ShowToast(String msg, Object context);
    }
}
