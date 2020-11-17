using FluentValidation;
using System;
using System.Linq.Expressions;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Validation
{
    public class EquipamentoValidator : AbstractValidator<PontoDeControle>
    {
        public EquipamentoValidator()
        {
            this.RuleFor<string>((Expression<Func<PontoDeControle, string>>)(x => x.Ambiente)).Must<PontoDeControle, string>((Func<string, bool>)(equip => equip != null)).WithMessage<PontoDeControle, string>("Parâmetro \"Ambiente\" obrigatório").WithErrorCode<PontoDeControle, string>("0001");
            this.When((Func<PontoDeControle, bool>)(x => x.Ambiente != null), (Action)(() => this.RuleFor<string>((Expression<Func<PontoDeControle, string>>)(x => x.Ambiente)).Must<PontoDeControle, string>((Func<string, bool>)(equip =>
        {
            if (!(equip.ToUpper() == "P"))
                return equip.ToUpper() == "T";
            return true;
        })).WithMessage<PontoDeControle, string>("Parâmetro \"Ambiente\" deve ser preenchido com T ou P").WithErrorCode<PontoDeControle, string>("0002")));
            this.RuleFor<string>((Expression<Func<PontoDeControle, string>>)(x => x.PontoControle)).NotNull<PontoDeControle, string>().WithMessage<PontoDeControle, string>("Parâmetro \"PontoControle\" obrigatório").WithErrorCode<PontoDeControle, string>("0003");
            this.When((Func<PontoDeControle, bool>)(x => !string.IsNullOrEmpty(x.PontoControle)), (Action)(() => this.RuleFor<string>((Expression<Func<PontoDeControle, string>>)(x => x.PontoControle)).MaximumLength<PontoDeControle>(8).WithMessage<PontoDeControle, string>("Parâmetro \"PontoControle\" deve ser informado com tamanho máximo de 8 caracteres").WithErrorCode<PontoDeControle, string>("0004")));
        }
    }
}
