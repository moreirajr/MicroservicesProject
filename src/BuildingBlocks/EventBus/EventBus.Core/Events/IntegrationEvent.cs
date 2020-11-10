using Newtonsoft.Json;
using System;

namespace EventBus.Core.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime dataCriacao)
        {
            Id = id;
            DataCriacao = dataCriacao;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime DataCriacao { get; private set; }
    }
}