using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monolith.Foundation.Mail.Framework
{
    public interface IMailQueue
    {
        public Task EnqueueAsync(
            string from,
            string to,
            string view,
            IDictionary<string, string> data);
    }
}
