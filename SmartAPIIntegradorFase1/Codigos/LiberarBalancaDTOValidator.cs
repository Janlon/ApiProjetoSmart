using FluentValidation;
using System;
using System.Linq.Expressions;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Validation
{
    public class LiberarBalancaDTOValidator : AbstractValidator<LiberarBalancaDTO>
    {
        public LiberarBalancaDTOValidator()
        {
            this.RuleFor<string>((Expression<Func<LiberarBalancaDTO, string>>)(x => x.Ambiente)).Must<LiberarBalancaDTO, string>((Func<string, bool>)(equip => equip != null)).WithMessage<LiberarBalancaDTO, string>("Parâmetro \"Ambiente\" obrigatório").WithErrorCode<LiberarBalancaDTO, string>("0001");
            this.When((Func<LiberarBalancaDTO, bool>)(x => x.Ambiente != null), (Action)(() => this.RuleFor<string>((Expression<Func<LiberarBalancaDTO, string>>)(x => x.Ambiente)).Must<LiberarBalancaDTO, string>((Func<string, bool>)(equip =>
        {
            if (!(equip.ToUpper() == "P"))
                return equip.ToUpper() == "T";
            return true;
        })).WithMessage<LiberarBalancaDTO, string>("Parâmetro \"Ambiente\" deve ser preenchido com T ou P").WithErrorCode<LiberarBalancaDTO, string>("0002")));
            this.RuleFor<string>((Expression<Func<LiberarBalancaDTO, string>>)(x => x.Balanca)).Must<LiberarBalancaDTO, string>((Func<string, bool>)(balanca =>
          {
              if (balanca != null)
                  return !string.IsNullOrWhiteSpace(balanca);
              return false;
          })).WithMessage<LiberarBalancaDTO, string>("Parâmetro \"balanca\" obrigatório").WithErrorCode<LiberarBalancaDTO, string>("0003");
            this.RuleFor<string>((Expression<Func<LiberarBalancaDTO, string>>)(x => x.NumOs)).Must<LiberarBalancaDTO, string>((Func<string, bool>)(numOs =>
          {
              if (numOs != null)
                  return !string.IsNullOrWhiteSpace(numOs);
              return false;
          })).WithMessage<LiberarBalancaDTO, string>("Parâmetro \"NUmOs\" obrigatório").WithErrorCode<LiberarBalancaDTO, string>("0004");
        }
    }
}
