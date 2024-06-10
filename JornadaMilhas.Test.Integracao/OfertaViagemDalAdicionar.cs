using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;

[Collection(nameof(ContextoCollection))]
public class OfertaViagemDalAdicionar
{
    private readonly JornadaMilhasContext _context;

    public OfertaViagemDalAdicionar(ITestOutputHelper output,
        ContextoFixture fixture)
    {
        _context = fixture.Context;
        output.WriteLine(_context.GetHashCode().ToString());
    }

    private OfertaViagem CriaOfertaBase() 
    {
        Rota rota = new Rota("São Paulo", "Fortaleza");
        Periodo periodo = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 10));
        double preco = 350;

        return new(rota, periodo, preco);
    }

    [Fact]
    public void RegistraOfertaNoBanco()
    {
        OfertaViagem oferta = CriaOfertaBase();
        var dal = new OfertaViagemDAL(_context);

        dal.Adicionar(oferta);

        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);

        Assert.NotNull(ofertaIncluida);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
    }

    [Fact]
    public void RegistraOfertaNoBancoComInformacoesCorretas()
    {
        OfertaViagem oferta = CriaOfertaBase();
        var dal = new OfertaViagemDAL(_context);

        dal.Adicionar(oferta);

        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);

        Assert.Equal(ofertaIncluida.Rota.Origem, oferta.Rota.Origem);
        Assert.Equal(ofertaIncluida.Rota.Destino, oferta.Rota.Destino);
        Assert.Equal(ofertaIncluida.Periodo.DataInicial, oferta.Periodo.DataInicial);
        Assert.Equal(ofertaIncluida.Periodo.DataFinal, oferta.Periodo.DataFinal);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
    }
}