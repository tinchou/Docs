using System.Threading;
using FiltersSample.Interfaces;

namespace FiltersSample.Services
{
    public class DelayService :IDelayService
    {
        public void Delay()
        {
            Thread.Sleep(500);
        }
    }
}
