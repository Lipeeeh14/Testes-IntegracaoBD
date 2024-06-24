using Bogus;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test.Integracao;

public class RotaDataBuilder : Faker<Rota>
{
    public string? Origem { get; set; }
    public string? Destino { get; set; }

    public RotaDataBuilder()
    {
        CustomInstantiator(f => 
        {
            return new Rota(Origem ?? f.Address.City(),
                Destino ?? f.Address.City());
        });
    }

    public Rota Build() => Generate();
}
