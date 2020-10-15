using FCamara.Bussiness.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Bussiness.Interfaces.Service
{
    public interface IDependenteService : IDisposable
    {
        Task Adicionar(Dependente dependente);
        Task Atualizar(Dependente dependente);
        Task Remover(Guid id);
        Task<IEnumerable<Dependente>> ObterAniversariantes();
    }
}
