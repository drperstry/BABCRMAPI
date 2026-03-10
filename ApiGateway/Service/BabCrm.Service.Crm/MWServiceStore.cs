using BabCrm.Core;
using BabCrm.Crm;
using BabCrm.Service.Models;

namespace BabCrm.Service.Crm
{
    public class MWServiceStore : IMWExternalServiceStore
    {
        private MWExternalServices _mwService;

        public MWServiceStore(MWExternalServices mwService)
        {
            _mwService = mwService;
        }

        public async Task CallSendInstantNotification(SendInstantNotification notification)
        {
            try
            {
                await _mwService.Post(notification, "api/Notification/SendInstantNotification");

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
