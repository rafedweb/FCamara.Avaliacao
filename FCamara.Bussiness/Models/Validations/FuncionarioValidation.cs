using FCamara.Bussiness.Models.Validations.CPF;
using FluentValidation;

namespace FCamara.Bussiness.Models.Validations
{
    public class FuncionarioValidation : AbstractValidator<Funcionario>
    {
        public FuncionarioValidation()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(f => f.CPF.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("O campo CPF precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
            RuleFor(f => CpfValidacao.Validar(f.CPF)).Equal(true)
                .WithMessage("O CPF fornecido é inválido.");

        }
    }
}
