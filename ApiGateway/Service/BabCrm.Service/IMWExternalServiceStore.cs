using BabCrm.Core;
using BabCrm.Service.Models;

namespace BabCrm.Service
{
    public interface IMWExternalServiceStore
    {
        Task CallSendInstantNotification(SendInstantNotification notification);
    }
}
