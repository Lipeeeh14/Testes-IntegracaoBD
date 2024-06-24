using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace JornadaMilhas.Test.Integracao;

public class ContextoFixture : IAsyncLifetime
{
    /*
        Para cada teste é criado uma nova instancia da classe de teste para a sua execução.
        Então, para cada teste, também está sendo criado uma nova conexão com o banco de dados.
        Para evitar esse cenário e consumir menos recurso, essa classe foi criada.
        
        Utilizando a implementação do ClassFixture 
        garante os mesmos recursos para os testes de UMA ÚNICA CLASSE

        Utilizando a implementação do CollectionFixture 
        garante os mesmos recursos para os testes para MAIS DE UMA CLASSE
     */
    public JornadaMilhasContext Context { get; private set; }
    private readonly MsSqlContainer _msSqlContainer =
        new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        Context = new JornadaMilhasContext(new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
        .Options);
        Context.Database.Migrate();
    }

    public void CriaDadosFake()
    {
        Periodo periodo = new PeriodoDataBuilder().Build();

        var rota = new RotaDataBuilder().Build();

        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem(
                rota,
                new PeriodoDataBuilder().Build(),
                100 * f.Random.Int(1, 100))
            )
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var dal = new OfertaViagemDAL(Context);
        var lista = fakerOferta.Generate(200);

        Context.OfertasViagem.AddRange(lista);
        Context.SaveChanges();
    }

    public async Task LimpaDadosDoBanco() 
    {
        Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
        Context.Database.ExecuteSqlRaw("DELETE FROM Rotas");
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}
