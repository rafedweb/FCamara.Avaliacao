using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCamara.App.Models
{
    public class AniversarianteViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
    }
}
