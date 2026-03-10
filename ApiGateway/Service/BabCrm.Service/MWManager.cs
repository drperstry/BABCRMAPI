using BabCrm.Core;
using BabCrm.Service.Models;

namespace BabCrm.Service
{
    public class MWManager
    {
        private IMWExternalServiceStore _store;

        public MWManager(IMWExternalServiceStore store)
        {
            _store = store;
        }

        public async Task CallSendInstantNotification(SendInstantNotification notification)
        {
            try
            {
                await _store.CallSendInstantNotification(notification);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
