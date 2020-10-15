using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FCamara.App.Models;
using FCamara.Bussiness.Interfaces.Repository;
using AutoMapper;
using FCamara.Bussiness.Interfaces.Service;
using FCamara.Common.RequestFilter;

namespace FCamara.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFuncionarioService _funcionarioService;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IDependenteService _dependenteService;
        private readonly IMapper _mapper;
              

        public HomeController(ILogger<HomeController> logger,
                              IFuncionarioService funcionarioService,
                              IFuncionarioRepository funcionarioRepository,
                              IDependenteService dependenteService,
                              IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _funcionarioService = funcionarioService;
            _funcionarioRepository = funcionarioRepository;
            _dependenteService = dependenteService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Aniversariantes"] = await ObterAniversariantes();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Pesquisar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pesquisar(FiltrosFuncionario funcionarioViewModel)
        {
            var parametros  = new FiltrosFuncionario {
                Nome = (string.IsNullOrEmpty(funcionarioViewModel.Nome) ? string.Empty : funcionarioViewModel.Nome),
                Ativo = funcionarioViewModel.Ativo,
                Sexo = funcionarioViewModel.Sexo,
                Nascimento = funcionarioViewModel.Nascimento
            };

            var funcionario = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioService.ObterFuncionarios(parametros));

            return View();
        }

        public async Task<IEnumerable<AniversarianteViewModel>> ObterAniversariantes()
        {

            var funcionarios = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioService.ObterAniversariantes());
            var dependentes = _mapper.Map<IEnumerable<DependenteViewModel>>(await _dependenteService.ObterAniversariantes());

            var aniversariantes = new List<AniversarianteViewModel>();

            foreach (var funcionario in funcionarios)
            {
                var aniversariante = new AniversarianteViewModel
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Nascimento = funcionario.Nascimento
                };

                aniversariantes.Add(aniversariante);
            }

            foreach (var dependente in dependentes)
            {
                var aniversariante = new AniversarianteViewModel
                {
                    Id = dependente.Id,
                    Nome = dependente.Nome,
                    Nascimento = dependente.Nascimento
                };

                aniversariantes.Add(aniversariante);
            }

            return aniversariantes;
        }
    }
}
