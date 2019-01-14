using System;

namespace MMS.Model {
    public class LineItem {
        public string ID { get; set; }
        public string TrackingNumber { get; set; }
        public string RingTo { get; set; }
        public string TrackingLineName { get; set; }
        public string Type { get; set; }
        
        //Not in DB
        public string Label01 { get; set; }
        public string Label02 { get; set; }
        public string Label03 { get; set; }
        public string Label04 { get; set; }
        public string Label05 { get; set; }
        public string Label06 { get; set; }
        public string Label07 { get; set; }
        public string Label08 { get; set; }
        public string Label09 { get; set; }
        public string Label10 { get; set; }
        public string LabelValue01 { get; set; }
        public string LabelValue02 { get; set; }
        public string LabelValue03 { get; set; }
        public string LabelValue04 { get; set; }
        public string LabelValue05 { get; set; }
        public string LabelValue06 { get; set; }
        public string LabelValue07 { get; set; }
        public string LabelValue08 { get; set; }
        public string LabelValue09 { get; set; }
        public string LabelValue10 { get; set; }

        //Database Value Columns instead of label
        public string AccountNumber { get; set; }
        public string Advertiser { get; set; }
        public string Heading { get; set; }
        public string UDAC { get; set; }
        public string PubDate { get; set; }
        public string EndPubDate { get; set; }
        public string OriginalStartDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}