using Client.ServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class LessonVM
    {
        public int Id { get; set; }
        public string Adı { get; set; }
        public int LessonId { get; set; }
        public int VizeTahmin { get; set; }
        public int Vize { get; set; }
        public int FinalTahmin { get; set; }
        public int Final { get; set; }
        public int Büt { get; set; }
        public int BütTahmin { get; set; }
        public string VizeDurum { get; set; }
        public string FinalDurum { get; set; }
        public int StudentId { get; set; }
        public int GecmeNotu { get; set; }
        public double GecerNot { get; set; }
        public List<Lesson> LessonList { get; set; }
    }
}