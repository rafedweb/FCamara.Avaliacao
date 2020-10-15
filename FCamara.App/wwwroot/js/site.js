var FiltrosConsulta = {};

$(document).ready(function () {
    $('#Nome').val(FiltrosConsulta.Nome),
    $('#CPF').val(FiltrosConsulta.CPF),
    $('#Nascimento').val(FiltrosConsulta.Nascimento),
    $('#Sexo').val(FiltrosConsulta.Sexo),
    $('#Ativo').val(FiltrosConsulta.Ativo),
    $('#Dependentes').val(FiltrosConsulta.Dependentes)
});


function AjaxModal() {

    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal({
                                keyboard: true
                            },
                                'show');
                            bindForm(this);
                        });
                    return false;
                });
        });

        function bindForm(dialog) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide');
                            $('#EnderecoTarget').load(result.url);
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
                return false;
            });
        }
    });
}

function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            $("#Endereco_Logradouro").val("");
            $("#Endereco_Bairro").val("");
            $("#Endereco_Cidade").val("");
            $("#Endereco_Estado").val("");
        }

        $("#Endereco_Cep").blur(function () {

            var cep = $(this).val().replace(/\D/g, '');

            if (cep != "") {

                var validacep = /^[0-9]{8}$/;

                if (validacep.test(cep)) {

                    $("#Endereco_Logradouro").val("...");
                    $("#Endereco_Bairro").val("...");
                    $("#Endereco_Cidade").val("...");
                    $("#Endereco_Estado").val("...");

                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                $("#Endereco_Logradouro").val(dados.logradouro);
                                $("#Endereco_Bairro").val(dados.bairro);
                                $("#Endereco_Cidade").val(dados.localidade);
                                $("#Endereco_Estado").val(dados.uf);
                            }
                            else {
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                }
                else {
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            }
            else {
                limpa_formulário_cep();
            }
        });
    });
}

$(document).ready(function () {
    $("#msg_box").fadeOut(2500);
});

function PesquisarFuncionario() {

    $(document).ready(function () {
        $(function () {          

            $("#pesquisar").on("click",
                function (e) {
                    GetFormData();
                    debugger
                    $.ajax({
                        url: this.baseURI,
                        type: 'post',                      
                        data: FiltrosConsulta,                       
                        success: function (result) {
                            if (result.success) {
                                console.log(result);                                
                            } else {                             
                                console.log(result);
                            }
                        }
                    }).fail(function (jqXHR, textStatus, msg) {
                        console.log(jqXHR);
                    });               
                     return false;
                });
        });
    });
}

function GetFormData() {

    FiltrosConsulta = {
        Nome: $('#Nome').val(),
        CPF: $('#CPF').val(),
        Nascimento: $('#Nascimento').val(),
        Sexo: $('#Sexo').val(),
        Ativo: $('#Ativo').val(),
        Dependentes: $('#Dependentes').val()
    }

    return false;
}