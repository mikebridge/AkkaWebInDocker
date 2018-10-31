using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public interface IEchoActorSystem
    {

        Task Send(string message);

    }
}
