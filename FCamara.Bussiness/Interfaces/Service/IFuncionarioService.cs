using FCamara.Bussiness.Models;
using FCamara.Common.RequestFilter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Bussiness.Interfaces.Service
{
    public interface IFuncionarioService : IDisposable
    {
        Task Adicionar(Funcionario funcionario);
        Task Atualizar(Funcionario funcionario);
        Task Remover(Guid id);

        Task AtualizarEndereco(Endereco endereco);
        Task<IEnumerable<Funcionario>> ObterAniversariantes();
        Task<IEnumerable<Funcionario>> ObterFuncionarios(FiltrosFuncionario filtros);
    }
}
