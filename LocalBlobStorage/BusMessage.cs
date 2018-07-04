using System;

namespace LocalBlobStorage
{
    class BusMessage
    {
        public Guid Id => Guid.NewGuid();
        public string TrackingId { get; set; }
    }
}
