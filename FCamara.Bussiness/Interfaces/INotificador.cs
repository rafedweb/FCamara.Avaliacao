using FCamara.Bussiness.Notificacoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCamara.Bussiness.Interfaces
{
   public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
