using JobManagementSystem.Entities;
using JobManagementSystem.Services;

namespace JobManagementSystem.Tests.Services;

/// <summary>
/// The TaxService has complicated logic in it that is not always easy to setup from other services.  Therefore the TaxService can be argued to warrant its own tests.
/// </summary>
public class TaxServiceTests
{
    [Fact]
    public void GetTaxLiabilities_ReturnsCountryCodes()
    {
        //Arrange
        var job = new Job() { Id = 1, Description = "Desc" };
        job.TaxInformation.Add(new TaxInformation
        {
            Id = 1,
            JobId = 1,
            Description = "Desc",
            TaxRegime = new TaxRegime
            {
                Id = 1,
                CountryCode = "GBR",
                Description = "Britain",
                MinimumThreshold = 10000M,
                TaxRate = 0.2M
            }
        });
        job.TaxInformation.Add(new TaxInformation
        {
            Id = 2,
            JobId = 1,
            Description = "Desc",
            TaxRegime = new TaxRegime
            {
                Id = 1,
                CountryCode = "USA",
                Description = "America",
                MinimumThreshold = 2000M,
                TaxRate = 0.1M
            }
        });

        var service = new TaxService();
        var expected = new List<string>() { "GBR", "USA" };

        //Act
        var actual = service.GetTaxLiabilities(job);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTaxLiabilities_ReturnsEmptyListForNoData()
    {
        var job = new Job() { Id = 1, Description = "Desc" };
        var service = new TaxService();

        //Act
        var actual = service.GetTaxLiabilities(job);

        //Assert
        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void GetIndicativeTaxPercentage_ReturnsCorrectValue_SingleRegime()
    {
        //Arrange
        var service = new TaxService();
        List<TaxRegime> regimes = [
            new(){ Id = 1, CountryCode = "GBR", Description = "GBR", MinimumThreshold = 10000, TaxRate = 0.2M }
        ];

        //Act
        var actual = service.GetIndicativeTaxPercentage(20000, regimes);
        var expected = 2000M / 20000M;

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetIndicativeTaxPercentage_ReturnsCorrectValueWhenBelowThreshold_SingleRegime()
    {
        //Arrange
        var service = new TaxService();
        List<TaxRegime> regimes = [
            new(){ Id = 1, CountryCode = "GBR", Description = "GBR", MinimumThreshold = 10000, TaxRate = 0.2M }
        ];

        //Act
        var actual = service.GetIndicativeTaxPercentage(5000, regimes);
        var expected = 0M;

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetIndicativeTaxPercentage_ReturnsCorrectValue_MultipleRegimes()
    {
        //Arrange
        var service = new TaxService();
        List<TaxRegime> regimes = [
            new(){ Id = 1, CountryCode = "GBR", Description = "GBR", MinimumThreshold = 10000, TaxRate = 0.2M },
            new(){ Id = 2, CountryCode = "USA", Description = "USA", MinimumThreshold = 5000, TaxRate = 0.1M },
        ];

        //Act
        var actual = service.GetIndicativeTaxPercentage(20000, regimes);
        var expected = (2000M + 1500M) / 20000M;

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetIndicativeTaxPercentage_ReturnsCorrectValueWhenOneBelowThreshold_MultipleRegimes()
    {
        //Arrange
        var service = new TaxService();
        List<TaxRegime> regimes = [
            new(){ Id = 1, CountryCode = "GBR", Description = "GBR", MinimumThreshold = 10000, TaxRate = 0.2M },
            new(){ Id = 2, CountryCode = "USA", Description = "USA", MinimumThreshold = 5000, TaxRate = 0.1M },
        ];

        //Act
        var actual = service.GetIndicativeTaxPercentage(9000, regimes);
        var expected = 400M/9000M;

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetIndicativeTaxPercentage_ReturnsCorrectValueWhenAllBelowThreshold_MultipleRegimes()
    {
        //Arrange
        var service = new TaxService();
        List<TaxRegime> regimes = [
            new(){ Id = 1, CountryCode = "GBR", Description = "GBR", MinimumThreshold = 10000, TaxRate = 0.2M },
            new(){ Id = 2, CountryCode = "USA", Description = "USA", MinimumThreshold = 5000, TaxRate = 0.1M },
        ];

        //Act
        var actual = service.GetIndicativeTaxPercentage(4000, regimes);
        var expected = 0M;

        //Assert
        Assert.Equal(expected, actual);
    }
}
