using FluentValidation;
using System;
using System.Linq.Expressions;
using WebApiBusiness.App_Data;
using WebApiBusiness.Util;

namespace WebApiBusiness.Validation
{
    public class EntradaNaoAutorizadaDTOValidator : AbstractValidator<EntradaNaoAutorizadaDTO>
    {
        public EntradaNaoAutorizadaDTOValidator()
        {
            this.RuleFor<string>((Expression<Func<EntradaNaoAutorizadaDTO, string>>)(x => x.Ambiente)).Must<EntradaNaoAutorizadaDTO, string>((Func<string, bool>)(equip => equip != null)).WithMessage<EntradaNaoAutorizadaDTO, string>("Parâmetro \"Ambiente\" obrigatório").WithErrorCode<EntradaNaoAutorizadaDTO, string>("0001");
            this.When((Func<EntradaNaoAutorizadaDTO, bool>)(x => x.Ambiente != null), (Action)(() => this.RuleFor<string>((Expression<Func<EntradaNaoAutorizadaDTO, string>>)(x => x.Ambiente)).Must<EntradaNaoAutorizadaDTO, string>((Func<string, bool>)(equip =>
        {
            if (!(equip.ToUpper() == "P"))
                return equip.ToUpper() == "T";
            return true;
        })).WithMessage<EntradaNaoAutorizadaDTO, string>("Parâmetro \"Ambiente\" deve ser preenchido com T ou P").WithErrorCode<EntradaNaoAutorizadaDTO, string>("0002")));
            this.When((Func<EntradaNaoAutorizadaDTO, bool>)(x => !string.IsNullOrEmpty(x.CPF)), (Action)(() => this.RuleFor<string>((Expression<Func<EntradaNaoAutorizadaDTO, string>>)(x => x.CPF)).MaximumLength<EntradaNaoAutorizadaDTO>(11).WithMessage<EntradaNaoAutorizadaDTO, string>("Parâmetro \"CPF\" deve ser informado com tamanho máximo de 11 caracteres").WithErrorCode<EntradaNaoAutorizadaDTO, string>("0003")));
            this.When((Func<EntradaNaoAutorizadaDTO, bool>)(x => !string.IsNullOrEmpty(x.PlacaOs)), (Action)(() => this.RuleFor<string>((Expression<Func<EntradaNaoAutorizadaDTO, string>>)(x => x.PlacaOs)).Must<EntradaNaoAutorizadaDTO, string>((Func<string, bool>)(placaOs => placaOs.IsPlate())).WithMessage<EntradaNaoAutorizadaDTO, string>("Parâmetro \"PlacaOS\" com formato inválido").WithErrorCode<EntradaNaoAutorizadaDTO, string>("0004")));
            this.When((Func<EntradaNaoAutorizadaDTO, bool>)(x => !string.IsNullOrEmpty(x.PlacaOcr)), (Action)(() => this.RuleFor<string>((Expression<Func<EntradaNaoAutorizadaDTO, string>>)(x => x.PlacaOcr)).Must<EntradaNaoAutorizadaDTO, string>((Func<string, bool>)(placaOcr => placaOcr.IsPlate())).WithMessage<EntradaNaoAutorizadaDTO, string>("Parâmetro \"PlacaOCR\" com formato inválido").WithErrorCode<EntradaNaoAutorizadaDTO, string>("0005")));
            this.When((Func<EntradaNaoAutorizadaDTO, bool>)(x =>
           {
               if (!string.IsNullOrEmpty(x.NumOs) && !string.IsNullOrWhiteSpace(x.NumOs) || !string.IsNullOrEmpty(x.PlacaOcr) && !string.IsNullOrWhiteSpace(x.PlacaOcr) || !string.IsNullOrEmpty(x.PlacaOs) && !string.IsNullOrWhiteSpace(x.PlacaOs))
                   return false;
               if (!string.IsNullOrEmpty(x.CPF))
                   return string.IsNullOrWhiteSpace(x.CPF);
               return true;
           }), (Action)(() => this.RuleFor<string>((Expression<Func<EntradaNaoAutorizadaDTO, string>>)(x => x.Ambiente)).Must<EntradaNaoAutorizadaDTO, string>((Func<string, bool>)(ambiente => ambiente == "5a9ba14e-0aca-446cSucess-987c-8697620d9754")).WithMessage<EntradaNaoAutorizadaDTO, string>("Nenhum parâmetro informado, logo nenhuma alteração realizada").WithErrorCode<EntradaNaoAutorizadaDTO, string>("0006")));
        }
    }
}
