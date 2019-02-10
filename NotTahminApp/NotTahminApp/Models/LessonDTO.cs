using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NotTahminApp.Models
{
    public class LessonDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int StudentId { get; set; }
        [DataMember]
        public string Adı { get; set; }
        [DataMember]
        public int VizeTahmin { get; set; }
        [DataMember]
        public int Vize { get; set; }
        [DataMember]
        public int Final { get; set; }
        [DataMember]
        public int FinalTahmin { get; set; }
        [DataMember]
        public int Büt { get; set; }
        [DataMember]
        public int BütTahmin { get; set; }
        [DataMember]
        public string VizeDurum { get; set; }
        [DataMember]
        public string FinalDurum { get; set; }
        [DataMember]
        public int GecmeNotu { get; set; }
        [DataMember]
        public double GecerNot { get; set; }
    }
}