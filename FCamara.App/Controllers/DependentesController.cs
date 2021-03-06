﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FCamara.App.Models;
using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Interfaces.Service;
using AutoMapper;
using FCamara.Bussiness.Interfaces;
using FCamara.Bussiness.Models;
using KissLog;

namespace FCamara.App.Controllers
{
    public class DependentesController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IDependenteRepository _dependenteRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IDependenteService _dependenteService;
        private readonly IMapper _mapper;

        public DependentesController(ILogger logger,
                                     IDependenteRepository dependenteRepository,
                                     IFuncionarioRepository funcionarioRepository,
                                     IMapper mapper,
                                     IDependenteService dependenteService, 
                                     INotificador notificador) : base(notificador)
        {
            _logger = logger;
            _dependenteRepository = dependenteRepository;
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
            _dependenteService = dependenteService;
        }
        
        [Route("lista-de-dependentes")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<DependenteViewModel>>(await _dependenteRepository.ObterDependentesFuncionarios()));
        }

        [Route("dados-do-dependente/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var dependeteViewModel = await ObterDependente(id);

            if (dependeteViewModel == null)
            {
                return NotFound();
            }

            return View(dependeteViewModel);
        }

        [Route("novo-dependente")]
        public async Task<IActionResult> Create()
        {
            var dependenteViewModel = await PopularFuncionarios(new DependenteViewModel());

            return View(dependenteViewModel);
        }

        [Route("novo-dependente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DependenteViewModel dependenteViewModel)
        {
            dependenteViewModel = await PopularFuncionarios(dependenteViewModel);
            if (!ModelState.IsValid) return View(dependenteViewModel);  
           
            await _dependenteService.Adicionar(_mapper.Map<Dependente>(dependenteViewModel));

            if (!OperacaoValida()) return View(dependenteViewModel);

            TempData["Sucesso"] = "Dependente adicionado com sucesso!";

            return RedirectToAction("Index");
        }

        [Route("editar-dependente/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dependenteViewModel = await ObterDependente(id);

            if (dependenteViewModel == null)
            {
                return NotFound();
            }

            return View(dependenteViewModel);
        }

        [Route("editar-dependente/{id:guid}")]
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

            TempData["Sucesso"] = "Dependente editado com sucesso!";

            return RedirectToAction("Index");

        }

        [Route("excluir-dependente/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dependente = await ObterDependente(id);

            if (dependente == null)
            {
                return NotFound();
            }

            return View(dependente);
        }

        [Route("excluir-dependente/{id:guid}")]
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
