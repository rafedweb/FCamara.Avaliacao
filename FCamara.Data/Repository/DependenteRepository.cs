using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Models;
using FCamara.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Data.Repository
{
    public class DependenteRepository : Repository<Dependente>, IDependenteRepository
    {
        public DependenteRepository(MeuDbContext context) : base(context)
        {
                
        }              

        public async Task<Dependente> ObterDependenteFuncionario(Guid id)
        {
            return await Db.Dependentes.AsNoTracking().Include(f => f.Funcionario)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Dependente>> ObterDependentesFuncionarios()
        {
            return await Db.Dependentes.AsNoTracking().Include(f => f.Funcionario)
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Dependente>> ObterDependentesPorFuncionario(Guid funcionarioId)
        {
            return await Buscar(p => p.FuncionarioId == funcionarioId);
        }
    }
}
