using functions.GetAllProjects;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend_portfolio.Tests;

public class GetProjectsTests
{
    [Fact]
    public async Task GetProjects_LegeDatabase_GeeftOkMetLegeLijst()
    {
        //arrange
        using var db = TestDb.Create();

        //act
        var result = await GetProjectsHandler.GetProjectsHandle(db, CancellationToken.None);

        //assert
        var ok = Assert.IsType<Ok<List<GetProjectsResponse>>>(result);
        Assert.Empty(ok.Value!);
        

    }


    [Fact]
    public async Task OnePlusOne()
    {
        //arrange
        var one = 1;

        //act
        var result = one + 1;

        //assert
        Assert.Equal(2, result);
    }


}