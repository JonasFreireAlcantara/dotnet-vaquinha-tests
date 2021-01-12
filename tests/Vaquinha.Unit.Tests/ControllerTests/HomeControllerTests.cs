using Microsoft.Extensions.Logging;
using Moq;
using Vaquinha.MVC.Controllers;
using Vaquinha.Domain;
using NToastNotify;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vaquinha.Domain.ViewModels;
using Vaquinha.Service;
using AutoMapper;
using System;

namespace Vaquinha.Unit.Tests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _logger = new Mock<ILogger<HomeController>>();
        private readonly Mock<IToastNotification> _toastNotification = new Mock<IToastNotification>();
        private readonly Mock<IHomeInfoService> _homeInfoService = new Mock<IHomeInfoService>();

        private HomeController _homeController;

        public HomeControllerTests()
        {
            Task<HomeViewModel> task = Task.FromResult(new HomeViewModel());
            _homeInfoService.Setup(h => h.RecuperarDadosIniciaisHomeAsync()).Returns(task);
        }

        [Fact]
        [Trait("HomeController", "HomeController_Index_RetornaViewCorreta")]
        public async void HomeController_Index_RetornaViewCorreta()
        {
            // Arrange
            _homeController = new HomeController(_logger.Object, _homeInfoService.Object, _toastNotification.Object);

            // Act
            var retorno = await _homeController.Index();

            // Assert
            retorno.Should().BeOfType<ViewResult>();
            _toastNotification.Verify(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()), Times.Never);

            var viewResult = (ViewResult) retorno;
            viewResult.Model.Should().BeOfType<HomeViewModel>();
        }

        [Fact]
        [Trait("HomeController", "HomeController_Privacy_RetornaViewCorreta")]
        public void HomeController_Privacy_RetornaViewCorreta()
        {
            // Arrange
            _homeController = new HomeController(_logger.Object, _homeInfoService.Object, _toastNotification.Object);

            // Act
            var retorno = _homeController.Privacy();

            // Assert
            retorno.Should().BeOfType<ViewResult>();
            _toastNotification.Verify(t => t.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()), Times.Never);

            var viewResult = (ViewResult) retorno;
            viewResult.Model.Should().BeNull();
        }
    }
}
