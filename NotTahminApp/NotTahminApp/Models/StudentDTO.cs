using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NotTahminApp.Models
{
    public class StudentDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string AdSoyad { get; set; }
        [DataMember]
        public string EMail { get; set; }
        [DataMember]
        public string KullaniciAdi { get; set; }
        [DataMember]
        public string Sifre { get; set; }
        [DataMember]
        public String GizliSoru { get; set; }
        [DataMember]
        public string GizliSoruCevap { get; set; }
        [DataMember]
        public bool Oturum { get; set; }
        [DataMember]
        public int DersSayisi { get; set; }
    }
}