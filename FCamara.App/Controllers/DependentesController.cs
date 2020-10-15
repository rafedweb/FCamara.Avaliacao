using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FCamara.App.Models;
using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Interfaces.Service;
using AutoMapper;
using FCamara.Bussiness.Interfaces;
using FCamara.Bussiness.Models;

namespace FCamara.App.Controllers
{
    public class DependentesController : BaseController
    {
        private readonly IDependenteRepository _dependenteRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IDependenteService _dependenteService;
        private readonly IMapper _mapper;

        public DependentesController(IDependenteRepository dependenteRepository,
                                     IFuncionarioRepository funcionarioRepository,
                                     IMapper mapper,
                                     IDependenteService dependenteService, 
                                     INotificador notificador) : base(notificador)
        {
            _dependenteRepository = dependenteRepository;
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
            _dependenteService = dependenteService;
        }

        // GET: Dependentes
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<DependenteViewModel>>(await _dependenteRepository.ObterDependentesFuncionarios()));
        }

        // GET: Dependentes/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var dependeteViewModel = await ObterDependente(id);

            if (dependeteViewModel == null)
            {
                return NotFound();
            }

            return View(dependeteViewModel);
        }

        // GET: Dependentes/Create
        public async Task<IActionResult> Create()
        {
            var dependenteViewModel = await PopularFuncionarios(new DependenteViewModel());

            return View(dependenteViewModel);
        }

        // POST: Dependentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DependenteViewModel dependenteViewModel)
        {
            dependenteViewModel = await PopularFuncionarios(dependenteViewModel);
            if (!ModelState.IsValid) return View(dependenteViewModel);  
           
            await _dependenteService.Adicionar(_mapper.Map<Dependente>(dependenteViewModel));

            if (!OperacaoValida()) return View(dependenteViewModel);

            return RedirectToAction("Index");
        }

        // GET: Dependentes/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var dependenteViewModel = await ObterDependente(id);

            if (dependenteViewModel == null)
            {
                return NotFound();
            }

            return View(dependenteViewModel);
        }

        // POST: Dependentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DependenteViewModel dependenteViewModel)
        {
            if (id != dependenteViewModel.Id) return NotFound();

            var dependeteAtualizacao = await ObterDependente(id);
            dependenteViewModel.Funcionario = dependeteAtualizacao.Funcionario;          
            if (!ModelState.IsValid) return View(dependenteViewModel);



            dependeteAtualizacao.Nome = dependenteViewModel.Nome;
            dependeteAtualizacao.CPF = dependenteViewModel.CPF;
            dependeteAtualizacao.Nascimento = dependenteViewModel.Nascimento;
            dependeteAtualizacao.Sexo = dependenteViewModel.Sexo;

            await _dependenteService.Atualizar(_mapper.Map<Dependente>(dependeteAtualizacao));

            if (!OperacaoValida()) return View(dependenteViewModel);

            return RedirectToAction("Index");

        }

        // GET: Dependentes/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var dependente = await ObterDependente(id);

            if (dependente == null)
            {
                return NotFound();
            }

            return View(dependente);
        }

        // POST: Dependentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dependente = await ObterDependente(id);

            if (dependente == null)
            {
                return NotFound();
            }

            await _dependenteService.Remover(id);

            if (!OperacaoValida()) return View(dependente);

            TempData["Sucesso"] = "Dependente excluido com sucesso!";

            return RedirectToAction("Index");
        }     

        private async Task<DependenteViewModel> ObterDependente(Guid id)
        {
            var dependente = _mapper.Map<DependenteViewModel>(await _dependenteRepository.ObterDependenteFuncionario(id));
            dependente.Funcionarios = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioRepository.ObterTodos());
            return dependente;
        }

        private async Task<DependenteViewModel> PopularFuncionarios(DependenteViewModel dependente)
        {
            dependente.Funcionarios = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioRepository.ObterTodos());
            return dependente;
        }
    }
}
