using FCamara.Common.Enums;
using System;
using System.Collections.Generic;

namespace FCamara.Bussiness.Models
{
    public class Funcionario : Entity
    {
        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public string Telefone { get; set; }
        public ESexo Sexo { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<Dependente> Dependentes { get; set; }

        public bool Aniversariante(Funcionario funcionario)
        {
            return funcionario.Ativo && DateTime.Now.Month  == funcionario.Nascimento.Month;
        }
    }
}
