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
using KissLog;

namespace FCamara.App.Controllers
{
    public class FuncionariosController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IFuncionarioService _funcionariorService;
        private readonly IMapper _mapper;

        public FuncionariosController(ILogger logger, 
                                      IFuncionarioRepository funcionarioRepository,
                                      IMapper mapper,
                                      IFuncionarioService funcionarioService,
                                      INotificador notificador) : base(notificador)
        {
            _logger = logger;
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
            _funcionariorService = funcionarioService;
        }
       
        [Route("lista-de-funcionarios")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioRepository.ObterTodos()));
        }
        
        [Route("dados-do-funcionario/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var funcionarioViewModel = await ObterFuncionarioDependentesEndereco(id);

            if (funcionarioViewModel == null)
            {
                return NotFound();
            }

            return View(funcionarioViewModel);
        }

        [Route("novo-funcionario")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("novo-funcionario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FuncionarioViewModel funcionarioViewModel)
        {
            if (!ModelState.IsValid) return View(funcionarioViewModel);

            var funcionario = _mapper.Map<Funcionario>(funcionarioViewModel);
            await _funcionariorService.Adicionar(funcionario);

            if (!OperacaoValida()) return View(funcionarioViewModel);

            TempData["Sucesso"] = "Funcionario Adicionado com sucesso!";

            return RedirectToAction("Index");
        }

        [Route("editar-funcionario/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var funcionaioViewModel = await ObterFuncionarioDependentesEndereco(id);

            if (funcionaioViewModel == null)
            {
                return NotFound();
            }

            return View(funcionaioViewModel);
        }

        [Route("editar-funcionario/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FuncionarioViewModel funcionarioViewModel)
        {
            if (id != funcionarioViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(funcionarioViewModel);

            var funcionario = _mapper.Map<Funcionario>(funcionarioViewModel);
            await _funcionariorService.Atualizar(funcionario);

            if (!OperacaoValida()) return View(await ObterFuncionarioDependentesEndereco(id));

            TempData["Sucesso"] = "Funcionario editado com sucesso!";

            return RedirectToAction("Index");
        }

        [Route("excluir-funcionario/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var funcionarioViewModel = await ObterFuncionarioEndereco(id);

            if (funcionarioViewModel == null)
            {
                return NotFound();
            }

            return View(funcionarioViewModel);
        }

        [Route("excluir-funcionario/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var funcionario = await ObterFuncionarioEndereco(id);

            if (funcionario == null) return NotFound();

            await _funcionariorService.Remover(id);

            if (!OperacaoValida()) return View(funcionario);

            TempData["Sucesso"] = "Funcionario excluido com sucesso!";

            return RedirectToAction("Index");
        }

        [Route("obter-endereco-funcionario/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var funcionario = await ObterFuncionarioEndereco(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return PartialView("_DetalhesEndereco", funcionario);
        }

        [Route("atualizar-endereco-funcionario/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var funcionario = await ObterFuncionarioEndereco(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new FuncionarioViewModel { Endereco = funcionario.Endereco });
        }

        [Route("atualizar-endereco-funcionario/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(FuncionarioViewModel funcionarioViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("CPF");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", funcionarioViewModel);

            await _funcionariorService.AtualizarEndereco(_mapper.Map<Endereco>(funcionarioViewModel.Endereco));

            if (!OperacaoValida()) return PartialView("_AtualizarEndereco", funcionarioViewModel);

            var url = Url.Action("ObterEndereco", "Funcionarios", new { id = funcionarioViewModel.Endereco.FuncionarioId });
            return Json(new { success = true, url });
        }

        private async Task<FuncionarioViewModel> ObterFuncionarioEndereco(Guid id)
        {
            return _mapper.Map<FuncionarioViewModel>(await _funcionarioRepository.ObterFuncionarioEndereco(id));
        }

        private async Task<FuncionarioViewModel> ObterFuncionarioDependentesEndereco(Guid id)
        {
            return _mapper.Map<FuncionarioViewModel>(await _funcionarioRepository.ObterFuncionarioDependentesEndereco(id));
        }
    }
}
