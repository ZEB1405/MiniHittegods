using MiniHittegods.Domain;
using FluentAssertions;

namespace MiniHittegods.Tests;


public class FoundItemTests
{
    [Fact]
    public void NewItemShouldBeAvailable()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");

        //Act
        var status = item.Status;

        //Assert
        status.Should().Be(FoundItemStatus.Available);
    }

    [Fact]
    public void NewItemShouldHaveATitle()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");

        item.Title.Should().Be("Lost Wallet");
    }

    [Fact]
    public void TitleCanBe80Characters()
    {
        //Arrange
        var title = new string('a', 80);
        var item = new FoundItem(title, "Station");

        item.Title.Should().Be(title);
    }

    [Fact]
    public void TitleCannotBeMoreThan80Characters()
    {
        //Arrange
        var title = new string('a', 81);

        //Act
        Action act = () => new FoundItem(title, "Station");

        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Title cannot be more than 80 characters.");
    }

    [Fact]
    public void TitleCannotBeEmpty()
    {
        var act = () => new FoundItem("", "Station");

        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Title cannot be empty or whitespace.");
    }

    [Fact]
    public void FoundLocationCannotBeEmpty()
    {
        var act = () => new FoundItem("Lost Wallet", "");

        act.Should().Throw<ArgumentException>().WithMessage("Found location cannot be empty or whitespace.");
    }

    [Fact]
    public void DescriptionCannotBeEmpty()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        var description = item.Description;

        description.Should().BeNull();
    }

    [Fact]
    public void NewItemShouldHaveACategory()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        var category = item.Category;

        category.Should().BeNull();
    }

    [Fact]
    public void NewItemShouldHaveFoundAtUtc()
    {
        var before = DateTime.UtcNow;

        var item = new FoundItem("Lost Wallet", "Station");

        item.FoundAtUtc.Should().BeOnOrAfter(before);
        item.FoundAtUtc.Should().BeOnOrBefore(DateTime.UtcNow);
    }

    [Fact]
    public void NewItemShouldHaveAnId()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        item.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void ClaimedByShouldBeNullForNewItem()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        item.ClaimedBy.Should().BeNull();
    }

    [Fact]
    public void ClaimedAtUtcShouldBeNullForNewItem()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        item.ClaimedAtUtc.Should().BeNull();
    }

    [Fact]
    public void ReturnedAtUtcShouldBeNullForNewItem()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        item.ReturnedAtUtc.Should().BeNull();
    }

    [Fact]
    public void AvailableItemCanBeClaimed()
    {
        var item = new FoundItem("Lost Wallet", "Station");

        item.Status.Should().Be(FoundItemStatus.Available);

        //Act
        item.Claim("John Doe");

        //Assert
        item.Status.Should().Be(FoundItemStatus.Claimed);
        item.ClaimedBy.Should().Be("John Doe");
        item.ClaimedAtUtc.Should().NotBeNull();
        item.ReturnedAtUtc.Should().BeNull();
    }

    [Fact]
    public void ClaimedItemCanNotBeClaimedAgain()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");

        //Act
        Action act = () => item.Claim("Jane Doe");

        //Assert
        act.Should().Throw<InvalidOperationException>()
        .WithMessage("Item is already claimed.");
    }

    [Fact]
    public void ClaimedItemCanBeReturned()
    {
        // Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");

        // Act
        item.Return();

        // Assert
        item.Status.Should().Be(FoundItemStatus.Returned);
        item.ClaimedBy.Should().Be("John Doe");
        item.ClaimedAtUtc.Should().NotBeNull();
        item.ReturnedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void AvailableItemCanNotBeReturned()
    {
        // Arrange
        var item = new FoundItem("Lost Wallet", "Station");

        // Act
        Action act = () => item.Return();

        // Assert
        act.Should().Throw<InvalidOperationException>()
        .WithMessage("Item is not claimed.");
}   }
