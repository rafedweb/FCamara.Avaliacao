using FCamara.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCamara.Common.RequestFilter
{
    public class FiltrosFuncionario
    {      
                
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }
        public int? Sexo { get; set; }           
        public bool? Ativo { get; set; }
        public bool? Dependentes { get; set; }
        public List<FuncionariosResponse> Funcionarios { get; set; }
    }
}
