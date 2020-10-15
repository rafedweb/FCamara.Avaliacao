using FCamara.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCamara.Common.RequestFilter
{
    public class FuncionariosResponse
    {       
        public string Nome { get; set; }        
        public string CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public ESexo Sexo { get; set; }      
        public bool Ativo { get; set; }
    }
}
