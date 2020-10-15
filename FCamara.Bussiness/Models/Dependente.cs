using FCamara.Common.Enums;
using System;

namespace FCamara.Bussiness.Models
{
    public class Dependente : Entity
    {
        public Guid FuncionarioId { get; set; }


        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }       
        public ESexo Sexo { get; set; }

        public Funcionario Funcionario { get; set; }

        public bool Aniversariante(Dependente dependente)
        {
            return DateTime.Now.Month == dependente.Nascimento.Month;
        }
    }
}
