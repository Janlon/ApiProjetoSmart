using FluentValidation;
using System;
using System.Linq.Expressions;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Validation
{
    public class LogValidator : AbstractValidator<LogSmartApi>
    {
        public LogValidator()
        {
            this.RuleFor<string>((Expression<Func<LogSmartApi, string>>)(x => x.texto)).Must<LogSmartApi, string>((Func<string, bool>)(equip => equip != null)).WithMessage<LogSmartApi, string>("Corpo da Requisição obrigatório").WithErrorCode<LogSmartApi, string>("0001");
            this.RuleFor<string>((Expression<Func<LogSmartApi, string>>)(x => x.metodo)).Must<LogSmartApi, string>((Func<string, bool>)(equip => equip != null)).WithMessage<LogSmartApi, string>("Metodo usado pela smartApi obrigatório").WithErrorCode<LogSmartApi, string>("0001");
        }
    }
}
