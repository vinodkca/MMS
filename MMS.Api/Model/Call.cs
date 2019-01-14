using System;

namespace MMS.Model {
    public class Call {
        public string ID { get; set; }
        public string TrackingNumber { get; set; }
        public string RingTo { get; set; }
        public string Caller { get; set; }
        public DateTime CallTime { get; set; }
        public int CallDurationSeconds { get; set; }
        public string Result { get; set; }
        public string CallerName { get; set; }
        public string CallerCity { get; set; }
        public string CallerState { get; set; }
        public string CallerZip { get; set; }
        public string LineId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}