using MiniHittegods.Domain;
using FluentAssertions;
using MiniHittegods.Application;

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
        .WithMessage("Item is not available for claiming.");
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
    }

    [Fact]
    public async Task ClaimedItemCanNotBeDeleted()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);

        var service = new FoundItemService(repository);

        //Act
        Func<Task> act = () => service.DeleteItemAsync(item.Id);

        //Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Claimed item cannot be deleted.");
    }

    [Fact]
    public async Task AvailableItemCanBeDeleted()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);

        var service = new FoundItemService(repository);

        //Act
        Func<Task> act = () => service.DeleteItemAsync(item.Id);

        //Assert
        await act.Should().NotThrowAsync();
        var deletedItem = await repository.GetByIdAsync(item.Id);
        deletedItem.Should().BeNull();
    }

    [Fact]
    public async Task NonExistentItemCannotBeDeleted()
    {
        //Arrange
        var repository = new FakeFoundItemRepository();
        var service = new FoundItemService(repository);
        var nonExistentId = Guid.NewGuid();

        //Act
        Func<Task> act = () => service.DeleteItemAsync(nonExistentId);

        //Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Item not found.");
    }

    [Fact]
    public async Task ClaimItemShouldSetStatusAndClaimedByAndClaimedAtUtc()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);

        var service = new FoundItemService(repository);

        //Act
        await service.ClaimItemAsync(item.Id, "John Doe");

        //Assert
        var claimedItem = await repository.GetByIdAsync(item.Id);
        claimedItem.Should().NotBeNull();
        claimedItem!.Status.Should().Be(FoundItemStatus.Claimed);
        claimedItem.ClaimedBy.Should().Be("John Doe");
        claimedItem.ClaimedAtUtc.Should().NotBeNull();
        repository.UpdateCalled.Should().BeTrue();
    }

    [Fact]
    public async Task AvailableItemCannotBeReturned()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);
        var service = new FoundItemService(repository);

        //Act
        Func<Task> act = () => service.ReturnItemAsync(item.Id);
        

        //Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Item is not claimed.");

        var returnedItem = await repository.GetByIdAsync(item.Id);
        returnedItem.Should().NotBeNull();
        returnedItem!.ReturnedAtUtc.Should().BeNull();
    }

    [Fact]
    public async Task ServiceCanReturnClaimedItem()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);
        var service = new FoundItemService(repository);

        //Act
        await service.ReturnItemAsync(item.Id);

        //Assert
        var returnedItem = await repository.GetByIdAsync(item.Id);
        returnedItem.Should().NotBeNull();
        returnedItem!.Status.Should().Be(FoundItemStatus.Returned);
        returnedItem.ReturnedAtUtc.Should().NotBeNull();
        repository.UpdateCalled.Should().BeTrue();
    }

    [Fact]
    public async Task NonExistentItemCannotBeClaimed()
    {
        //Arrange
        var repository = new FakeFoundItemRepository();
        var service = new FoundItemService(repository);
        var nonExistentId = Guid.NewGuid();

        //Act
        Func<Task> act = () => service.ClaimItemAsync(nonExistentId, "John Doe");

        //Assert
        await act.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Item not found.");
    }

    [Fact]
    public async Task AlreadyClaimedItemCannotBeClaimedAgain()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);
        var service = new FoundItemService(repository);

        //Act
        Func<Task> act = () => service.ClaimItemAsync(item.Id, "Jane Doe");

        //Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Item is not available for claiming.");
    }

    [Fact]
    public async Task ReturnedItemCannotBeClaimedAgain()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");
        item.Return();

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(item);
        var service = new FoundItemService(repository);

        //Act
        Func<Task> act = () => service.ClaimItemAsync(item.Id, "Jane Doe");

        //Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Item is not available for claiming.");
    }

    [Fact]
    public void ReturnedItemCannotBeClaimed()
    {
        //Arrange
        var item = new FoundItem("Lost Wallet", "Station");
        item.Claim("John Doe");
        item.Return();

        //Act
        Action act = () => item.Claim("Jane Doe");

        //Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Item is not available for claiming.");
    }

    [Fact]
    public async Task GetAllItemsCanFilterByStatus()
    {
        //Arrange
        var availableItem = new FoundItem("Lost Wallet", "Station");
        var claimedItem = new FoundItem("Lost Phone", "Park");
        claimedItem.Claim("John Doe");

        var repository = new FakeFoundItemRepository();
        await repository.AddAsync(availableItem);
        await repository.AddAsync(claimedItem);

        var service = new FoundItemService(repository);

        //Act
        var items = await service.GetAllItemsAsync(FoundItemStatus.Available);

        //Assert
        items.Should().ContainSingle();
        items[0].Title.Should().Be("Lost Wallet");
    }
}  
