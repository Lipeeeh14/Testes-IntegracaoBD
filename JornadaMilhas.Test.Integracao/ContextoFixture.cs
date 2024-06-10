using JornadaMilhas.Dados;
using Microsoft.EntityFrameworkCore;

namespace JornadaMilhas.Test.Integracao;

public class ContextoFixture
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
    public JornadaMilhasContext Context { get; }

    public ContextoFixture()
    {
        Context = new JornadaMilhasContext(new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=JornadaMilhas;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False")
        .Options);
    }
}
