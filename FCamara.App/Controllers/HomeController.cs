using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FCamara.App.Models;
using FCamara.Bussiness.Interfaces.Repository;
using AutoMapper;
using FCamara.Bussiness.Interfaces.Service;
using FCamara.Common.RequestFilter;
using KissLog;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using FCamara.Common.Enums;

namespace FCamara.App.Controllers
{
    public class HomeController : Controller
    {
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
            var filtros = VerificarFiltros();
            return View(filtros);
        }

        [HttpPost]
        public async Task<IActionResult> Pesquisar(FiltrosFuncionario funcionarioViewModel)
        {
            RemoverFiltros();

            SetarFiltros(funcionarioViewModel);

            var funcionarios = _mapper.Map<IEnumerable<FuncionarioViewModel>>(await _funcionarioService.ObterFuncionarios(funcionarioViewModel)).ToList();

            funcionarioViewModel.Funcionarios = new List<FuncionariosResponse>();

            funcionarios.ForEach(f => funcionarioViewModel.Funcionarios.Add(FuncionarioViewModelToFuncionarioResponse(f)));

            return View(funcionarioViewModel);
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

        private FuncionariosResponse FuncionarioViewModelToFuncionarioResponse(FuncionarioViewModel funcionarioViewModel)
        {
            return new FuncionariosResponse
            {
                Nome = funcionarioViewModel.Nome,
                CPF = funcionarioViewModel.CPF,
                Ativo = funcionarioViewModel.Ativo,
                Nascimento = funcionarioViewModel.Nascimento,
                Sexo = funcionarioViewModel.Sexo
            };
        }

        #region cookies
        public string GetCookie(string key)
        {
            return Request.Cookies[key];
        }

        public void SetCookie(string key, string value, double? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(1);
            Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

        private void SetarFiltros(FiltrosFuncionario filtros)
        {

            if (!string.IsNullOrEmpty(filtros.Nome)) SetCookie("Nome", filtros.Nome, 15.0);

            if (!string.IsNullOrEmpty(filtros.CPF)) SetCookie("CPF", filtros.CPF, 15.0);

            if (!string.IsNullOrEmpty(filtros?.Inicio.ToString())) SetCookie("Inicio", filtros.Inicio.ToString(), 15.0);

            if (!string.IsNullOrEmpty(filtros?.Fim.ToString())) SetCookie("Fim", filtros.Fim.ToString(), 15.0);

            if (!string.IsNullOrEmpty(filtros?.Sexo.ToString())) SetCookie("Sexo", filtros.Sexo.ToString(), 15.0);

            if (!string.IsNullOrEmpty(filtros?.Ativo.ToString())) SetCookie("Ativo", filtros.Ativo.ToString(), 15.0);

            if (!string.IsNullOrEmpty(filtros?.Dependentes.ToString())) SetCookie("Dependentes", filtros.Dependentes.ToString(), 15.0);
        }

        private void RemoverFiltros()
        {

            if (!string.IsNullOrEmpty(Request.Cookies["Nome"])) RemoveCookie("Nome");

            if (!string.IsNullOrEmpty(Request.Cookies["CPF"])) RemoveCookie("CPF");

            if (!string.IsNullOrEmpty(Request.Cookies["Inicio"])) RemoveCookie("Inicio");

            if (!string.IsNullOrEmpty(Request.Cookies["Fim"])) RemoveCookie("Fim");

            if (!string.IsNullOrEmpty(Request.Cookies["Sexo"])) RemoveCookie("Sexo");

            if (!string.IsNullOrEmpty(Request.Cookies["Ativo"])) RemoveCookie("Ativo");

            if (!string.IsNullOrEmpty(Request.Cookies["Dependentes"])) RemoveCookie("Dependentes");
        }

        private FiltrosFuncionario VerificarFiltros()
        {
            string nome = null, cpf = null;
            DateTime? inicio = null, fim = null;
            int? sexo = null;
            bool? ativo = null, dependentes = null;

            if (!string.IsNullOrEmpty(Request.Cookies["Nome"])) nome = Request.Cookies["Nome"];

            if (!string.IsNullOrEmpty(Request.Cookies["CPF"])) cpf = Request.Cookies["CPF"];

            if (!string.IsNullOrEmpty(Request.Cookies["Inicio"])) inicio = DateTime.Parse(Request.Cookies["Inicio"]);

            if (!string.IsNullOrEmpty(Request.Cookies["Fim"])) fim = DateTime.Parse(Request.Cookies["Fim"]);

            if (!string.IsNullOrEmpty(Request.Cookies["Sexo"])) sexo = int.Parse(Request.Cookies["Sexo"]);

            if (!string.IsNullOrEmpty(Request.Cookies["Ativo"])) ativo = bool.Parse(Request.Cookies["Ativo"]);

            if (!string.IsNullOrEmpty(Request.Cookies["Dependentes"])) dependentes = bool.Parse(Request.Cookies["Dependentes"]);


            return new FiltrosFuncionario
            {
                Nome = nome,
                CPF = cpf,
                Inicio = inicio,
                Fim = fim,
                Sexo = sexo,
                Ativo = ativo,
                Dependentes = dependentes
            };
        }

        #endregion
    }
}
