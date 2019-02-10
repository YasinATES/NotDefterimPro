using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NotTahminApp
{
    [DataContract]
    public class Student
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }
        [DataMember]
        [Required]
        public string AdSoyad { get; set; }
        [DataMember]
        [Required]
        public string EMail { get; set; }
        [DataMember]
        [Required]
        public string KullaniciAdi { get; set; }
        [DataMember]
        [Required]
        public string Sifre { get; set; }
        [DataMember]
        [Required]
        public String GizliSoru { get; set; }
        [DataMember]
        [Required]
        public string GizliSoruCevap { get; set; }
        [DataMember]
        public int DersSayisi { get; set; }
        [DataMember]
        [Required]
        public bool Oturum { get; set; }
        [IgnoreDataMember]
        public virtual List<Lesson> Lesson { get; set; }
    }
}