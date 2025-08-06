using FluentValidation;
using PedidoService.Application.DTOs;
using PedidoService.Application.Validators.Resources;


namespace PedidoService.Application.Validators
{
    public class CriarPedidoDtoValidator : AbstractValidator<CriarPedidoDto>
    {
        public CriarPedidoDtoValidator()
        {
            RuleFor(x => x.ClienteId)
                .NotEmpty().WithMessage(ValidationMessages.ClienteIdObrigatorio);

            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage(ValidationMessages.ItensObrigatorios);

            RuleForEach(x => x.Itens).ChildRules(itens =>
            {
                itens.RuleFor(i => i.Produto)
                    .NotEmpty().WithMessage(ValidationMessages.ProdutoObrigatorio);

                itens.RuleFor(i => i.Quantidade)
                    .GreaterThan(0).WithMessage(ValidationMessages.QuantidadeMaiorQueZero);

                itens.RuleFor(i => i.PrecoUnitario)
                    .GreaterThan(0).WithMessage(ValidationMessages.PrecoMaiorQueZero);
            });
        }
    }
}