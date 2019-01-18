using System;

namespace MMS.Model {
    public class Line {
        public string ID { get; set; }
        public string TrackingNumber { get; set; }
        public string RingTo { get; set; }
        public string TrackingLineName { get; set; }
        public string Type { get; set; }
        
       //Database Value Columns instead of label
        public string AccountNumber { get; set; }
        public string Market { get; set; }
        public string Heading { get; set; }
        public string UDAC { get; set; }
        public string PubDate { get; set; }
        public string EndPubDate { get; set; }
        public string OriginalStartDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}