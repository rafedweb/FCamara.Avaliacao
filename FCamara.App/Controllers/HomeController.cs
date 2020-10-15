using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FCamara.App.Models;
using FCamara.Bussiness.Interfaces.Repository;
using AutoMapper;
using FCamara.Bussiness.Interfaces.Service;
using FCamara.Common.RequestFilter;
using KissLog;

namespace FCamara.App.Controllers
{
    public class HomeController : Controller
    {
        //log do dotNet Core
        //private readonly ILogger<HomeController> _logger;

        //Kiss logger
        private readonly ILogger _logger;
        private readonly IFuncionarioService _funcionarioService;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IDependenteService _dependenteService;
        private readonly IMapper _mapper;
              

        public HomeController(ILogger logger,
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

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;

                //Registro de log
                _logger.Error("Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.");
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;

                //Registro de Log
                _logger.Error("A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte");
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;

                //Registro de Log
                _logger.Error("Você não tem permissão para fazer isto.");
            }
            else
            {
                //Registro de Log
                _logger.Error("Ocorreu um Erro!");
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }

        [HttpGet]
        public IActionResult Pesquisar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pesquisar(FuncionarioViewModel funcionarioViewModel)
        {
            //var parametros  = new FiltrosFuncionario {
            //    Nome = (string.IsNullOrEmpty(funcionarioViewModel.Nome) ? string.Empty : funcionarioViewModel.Nome),
            //    Ativo = funcionarioViewModel.Ativo,
            //    Sexo = funcionarioViewModel.Sexo,
            //    Nascimento = funcionarioViewModel.Nascimento
            //};

            //var funcionario = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioService.ObterFuncionarios(parametros));

           // return PartialView("_ListaFuncionarios", funcionario);
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
