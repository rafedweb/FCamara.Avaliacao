using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCamara.Bussiness.Models.Validations
{
    public class DependenteValidation : AbstractValidator<Dependente>
    {
        public DependenteValidation()
        {
            RuleFor(f => f.Nome)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
