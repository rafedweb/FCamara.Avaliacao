using FCamara.Bussiness.Interfaces;
using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Interfaces.Service;
using FCamara.Bussiness.Models;
using FCamara.Bussiness.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.Bussiness.Services
{
    public class DependenteService : BaseService, IDependenteService
    {
        private readonly IDependenteRepository _dependenteRepository;
        public DependenteService(IDependenteRepository dependenteRepository, 
                                 INotificador notificador) : base(notificador)
        {
            _dependenteRepository = dependenteRepository;
        }

        public async Task Adicionar(Dependente dependente)
        {
            if (!ExecutarValidacao(new DependenteValidation(), dependente)) return;

            await _dependenteRepository.Adicionar(dependente);
        }

        public async Task Atualizar(Dependente dependente)
        {
            if (!ExecutarValidacao(new DependenteValidation(), dependente)) return;

            await _dependenteRepository.Atualizar(dependente);
        }

        public void Dispose()
        {
            _dependenteRepository?.Dispose();
        }

        public async Task Remover(Guid id)
        {
            await _dependenteRepository.Remover(id);
        }

        public async Task<IEnumerable<Dependente>> ObterAniversariantes()
        {
            var dependentes = await _dependenteRepository.ObterTodos();
            if(dependentes.Any()) return dependentes.Where(f => f.Aniversariante(f) == true).ToList();

            return new List<Dependente>();

        }
    }
}
