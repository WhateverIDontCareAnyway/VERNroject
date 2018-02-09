using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PRINGUP_Proj.Models
{
    public class Izvjestaj_metadata
    {
        [DisplayName("Date posted")]
        public string Date_Posted { get; set; }
        [DisplayName("Date edited")]
        public string Date_Edited { get; set; }
        [DisplayName("Duration")]
        public string Duration { get; set; }
        [DisplayName("Members")]
        public string Members_present { get; set; }
    }

    [MetadataType(typeof(Izvjestaj_metadata))]
    public partial class Izvjestaj
    {

    }
}