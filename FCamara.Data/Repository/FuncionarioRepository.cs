﻿using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Models;
using FCamara.Common.RequestFilter;
using FCamara.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Data.Repository
{
    public class FuncionarioRepository : Repository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(MeuDbContext context) : base(context)
        {
                
        }
     
        public async Task<Funcionario> ObterFuncionarioDependentesEndereco(Guid id)
        {
            return await Db.Funcionarios.AsNoTracking()
                .Include(c => c.Dependentes)
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Funcionario> ObterFuncionarioEndereco(Guid id)
        {
            return await Db.Funcionarios.AsNoTracking()
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Funcionario>> ObterFuncionarios(FiltrosFuncionario filtros)
        {

            var nome = (string.IsNullOrEmpty(filtros.Nome) ? string.Empty : filtros.Nome);
            var Nascimento = filtros.Nascimento;
            var depe

            var query = Db.Set<Funcionario>()
             .AsQueryable()
             .AsNoTracking();

            var lstResult = query.Select(x => x).Where(x => x.Nome.Contains(filtros.Nome)).ToList();
            return lstResult;
        }
    }
}
