using FCamara.Bussiness.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Bussiness.Interfaces.Repository
{
    public interface IFuncionarioRepository : IRepository<Funcionario>
    {
        Task<Funcionario> ObterFuncionarioEndereco(Guid id);

        Task<Funcionario> ObterFuncionarioDependentesEndereco(Guid id);               
    }
}
