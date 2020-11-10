namespace Contas.Application.ViewModels
{
    public class CartaoVM
    {
        public long ContaId { get; set; }

        public string NumeroCartao { get; set; }

        public bool Bloqueado { get; set; }
    }
}