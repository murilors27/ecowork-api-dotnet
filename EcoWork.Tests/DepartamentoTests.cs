using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

public class DepartamentoTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DepartamentoTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    public record DepartamentoCreateDto(string Nome);
    public record DepartamentoResponseDto(int Id, string Nome);

    [Fact]
    public async Task Get_DeveRetornarOk()
    {
        var response = await _client.GetAsync("/api/v1/departamentos");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_DeveCriarDepartamento()
    {
        var novoDepartamento = new DepartamentoCreateDto("Tecnologia");

        var response = await _client.PostAsJsonAsync("/api/v1/departamentos", novoDepartamento);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var dto = await response.Content.ReadFromJsonAsync<DepartamentoResponseDto>();

        dto.Should().NotBeNull();
        dto!.Nome.Should().Be("Tecnologia");
    }

    [Fact]
    public async Task GetById_DeveRetornarDepartamento()
    {
        var novo = new DepartamentoCreateDto("Financeiro");
        var created = await _client.PostAsJsonAsync("/api/v1/departamentos", novo);

        var criado = await created.Content.ReadFromJsonAsync<DepartamentoResponseDto>();
        criado.Should().NotBeNull();

        var response = await _client.GetAsync($"/api/v1/departamentos/{criado!.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var encontrado = await response.Content.ReadFromJsonAsync<DepartamentoResponseDto>();
        encontrado!.Id.Should().Be(criado.Id);
    }

    [Fact]
    public async Task Put_DeveAlterarDepartamento()
    {
        var novo = new DepartamentoCreateDto("RH");
        var created = await _client.PostAsJsonAsync("/api/v1/departamentos", novo);

        var criado = await created.Content.ReadFromJsonAsync<DepartamentoResponseDto>();
        criado.Should().NotBeNull();

        var update = new DepartamentoCreateDto("Recursos Humanos");

        var response = await _client.PutAsJsonAsync($"/api/v1/departamentos/{criado!.Id}", update);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var atualizado = await response.Content.ReadFromJsonAsync<DepartamentoResponseDto>();
        atualizado!.Nome.Should().Be("Recursos Humanos");
    }

    [Fact]
    public async Task Delete_DeveRemoverDepartamento()
    {
        var novo = new DepartamentoCreateDto("Marketing");
        var created = await _client.PostAsJsonAsync("/api/v1/departamentos", novo);
        var criado = await created.Content.ReadFromJsonAsync<DepartamentoResponseDto>();

        var response = await _client.DeleteAsync($"/api/v1/departamentos/{criado!.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var check = await _client.GetAsync($"/api/v1/departamentos/{criado.Id}");
        check.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}