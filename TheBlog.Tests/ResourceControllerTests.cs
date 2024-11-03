using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBlog.Common.Constants;
using TheBlog.Controllers;
using TheBlog.Interfaces;
using TheBlog.Models;

namespace TheBlog.Tests
{
    public class ResourceControllerTests
    {
        [Fact]
        public void Image_ReturnsValidImage_WhenIdIsValid()
        {
            // Arrange
            var expectedImage = new ImageFile
            {
                Id = "test-image-id",
                NameInStorage = "test-image.png",
                Bytes = new byte[] { 0x1, 0x2, 0x3, 0x4 },
            };
            var expectedContentType = "image/png";

            var mockResourceManager = new Mock<IResourceManager>();
            mockResourceManager.Setup(x => x.GetImage(expectedImage.Id)).Returns(expectedImage);

            var controller = new ResourceController(mockResourceManager.Object);

            // Act
            var result = controller.Image(expectedImage.Id);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(expectedContentType, fileResult.ContentType);
            Assert.Equal(expectedImage.NameInStorage, fileResult.FileDownloadName);
            Assert.Equal(expectedImage.Bytes, fileResult.FileContents);
        }


        [Fact]
        public void Image_ReturnsNotFound_WhenImageIdIsNull()
        {
            // Arrange
            var mockResourceManager = new Mock<IResourceManager>();
            var controller = new ResourceController(mockResourceManager.Object);

            // Act
            var result = controller.Image(null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            mockResourceManager.Verify(x => x.GetImage(It.IsAny<string>()), Times.Once());
            Assert.Equal(ResourceErrors.NotFound, notFoundResult.Value);
        }
    }
}
