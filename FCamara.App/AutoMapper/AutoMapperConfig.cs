using AutoMapper;
using FCamara.App.Models;
using FCamara.Bussiness.Models;

namespace FCamara.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Funcionario, FuncionarioViewModel>().ReverseMap();
            CreateMap<Dependente, DependenteViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();        
        }
    }
}
