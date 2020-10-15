using FCamara.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FCamara.App.Models
{
    public class DependenteViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Funcionario")]
        public Guid FuncionarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        public string CPF { get; set; }

        public DateTime Nascimento { get; set; }

        [DisplayName("Sexo")]
        public ESexo Sexo { get; set; }

        public FuncionarioViewModel Funcionario { get; set; }

        public IEnumerable<FuncionarioViewModel> Funcionarios { get; set; }

    }
}
