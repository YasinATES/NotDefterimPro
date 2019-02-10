using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class StudentsVM
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string EMail { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string GizliSoru { get; set; }
        public string GizliSoruCevap { get; set; }
        public bool Oturum { get; set; }
    }
}