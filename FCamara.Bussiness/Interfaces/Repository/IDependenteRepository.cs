using FCamara.Bussiness.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Bussiness.Interfaces.Repository
{
    public interface IDependenteRepository : IRepository<Dependente>
    {
        Task<IEnumerable<Dependente>> ObterDependentesPorFuncionario(Guid funcionarioId);
        Task<IEnumerable<Dependente>> ObterDependentesFuncionarios();
        Task<Dependente> ObterDependenteFuncionario(Guid id);        
    }
}
