using JornadaMilhas.Dados;
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

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}
