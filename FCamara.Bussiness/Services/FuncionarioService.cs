using FCamara.Bussiness.Interfaces;
using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Interfaces.Service;
using FCamara.Bussiness.Models;
using FCamara.Bussiness.Models.Validations;
using FCamara.Common.RequestFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCamara.Bussiness.Services
{
    public class FuncionarioService : BaseService, IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository,
                                 IEnderecoRepository enderecoRepository,
                                 INotificador notificador) : base(notificador)
        {
            _funcionarioRepository = funcionarioRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Funcionario funcionario)
        {
            if (!ExecutarValidacao(new FuncionarioValidation(), funcionario)
                || !ExecutarValidacao(new EnderecoValidation(), funcionario.Endereco)) return;

            await _funcionarioRepository.Adicionar(funcionario);
        }

        public async Task Atualizar(Funcionario funcionario)
        {
            if (!ExecutarValidacao(new FuncionarioValidation(), funcionario)) return;

            await _funcionarioRepository.Atualizar(funcionario);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task Remover(Guid id)
        {
            if (_funcionarioRepository.ObterFuncionarioDependentesEndereco(id).Result.Dependentes.Any())
            {
                Notificar("O funcionario possui dependentes cadastrados!");
                return;
            }

            var endereco = await _enderecoRepository.ObterEnderecoPorFuncionario(id);

            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }

            await _funcionarioRepository.Remover(id);
        }

        public void Dispose()
        {
            _funcionarioRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }

        public async Task<IEnumerable<Funcionario>> ObterAniversariantes()
        {
            var funcionarios = await _funcionarioRepository.ObterTodos();
            if (funcionarios.Any()) return funcionarios.Where(f => f.Aniversariante(f) == true).ToList();

            return new List<Funcionario>();
           
        }

        public async Task<IEnumerable<Funcionario>> ObterFuncionarios(FiltrosFuncionario filtros)
        {
            var funcionarios = await _funcionarioRepository.Buscar(f => f.Nome.Contains(filtros.Nome)
                                                  && f.Sexo == filtros.Sexo
                                                  && f.Ativo == filtros.Ativo
                                                  //&& f.Nascimento == funcionarioViewModel.Nascimento
                                                  );

            return funcionarios;
                      
            
        }
    }
}
