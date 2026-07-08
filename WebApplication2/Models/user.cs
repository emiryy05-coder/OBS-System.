using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class user
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Ad 3 ile 20 karakter arasında olmalıdır.")]
        public string Ad {  get; set; }
        
        public string Password { get; set; }
    }
}
