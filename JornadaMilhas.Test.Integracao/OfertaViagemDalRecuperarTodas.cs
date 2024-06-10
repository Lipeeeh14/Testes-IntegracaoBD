using JornadaMilhas.Dados;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

[Collection(nameof(ContextoCollection))]
public class OfertaViagemDalRecuperarTodas
{
    private readonly JornadaMilhasContext _context;

    public OfertaViagemDalRecuperarTodas(ITestOutputHelper output,
        ContextoFixture fixture)
    {
        _context = fixture.Context;
        output.WriteLine(_context.GetHashCode().ToString());
    }

    [Fact]
    public void ValidaRetornoNaoNulo()
    {
        var dal = new OfertaViagemDAL(_context);

        var ofertas = dal.RecuperarTodas();

        Assert.NotNull(ofertas);
    }
}
