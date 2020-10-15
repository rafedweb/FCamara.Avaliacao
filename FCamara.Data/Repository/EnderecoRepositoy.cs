using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Models;
using FCamara.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Data.Repository
{
    public class EnderecoRepositoy : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepositoy(MeuDbContext context) : base(context) { }

        public async Task<Endereco> ObterEnderecoPorFuncionario(Guid funcionarioId)
        {
            return await Db.Enderecos.AsNoTracking()
                 .FirstOrDefaultAsync(f => f.FuncionarioId == funcionarioId);
        }
    }
}
