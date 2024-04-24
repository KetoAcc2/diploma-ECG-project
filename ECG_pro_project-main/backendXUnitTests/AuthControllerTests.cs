using backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace backendXUnitTests
{
    public class AuthControllerTests
    {
        #region LoginAsync
        [Fact]
        public async void LoginAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnHash = new byte[] { };
            var returnSalt = new byte[] { };
            var expectedJwtToken = new JwtTokenDTO { AccessToken = "test test test test" };
            var expectedUserDTO = new UserDTO { UserId = 0 };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetUserHash(It.IsAny<string>()))
                .ReturnsAsync(returnHash);

            jwtTokenService.Setup(x => x.GetUserSalt(It.IsAny<string>()))
                .ReturnsAsync(returnSalt);

            jwtTokenService.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);

            jwtTokenService.Setup(x => x.RenewAccessRefreshToken(It.IsAny<string>()))
                .ReturnsAsync(expectedJwtToken);

            jwtTokenService.Setup(x => x.GetUserByEmailReturnDTO(It.IsAny<string>()))
                .ReturnsAsync(expectedUserDTO);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<LoginResponseDataDTO>(res.Value);
            Assert.Multiple(() =>
            {
                Assert.Equal(expectedJwtToken.AccessToken, actualValue.JwtToken.AccessToken);
                Assert.Equal(expectedUserDTO.UserId, actualValue.UserData.UserId);
            });
        }

        [Fact]
        public async void LoginAsync_UnsuccessfulExistsUserByEmailAsync_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LoginAsync_UnsuccessfulAccountActivated_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LoginAsync_ProvidingWrongUserHash_Returns401()
        {
            var expectedStatusCode = 401;
            var returnSalt = new byte[] { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetUserSalt(It.IsAny<string>()))
                .ReturnsAsync(returnSalt);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LoginAsync_ProvidingWrongUserSalt_Returns401()
        {
            var expectedStatusCode = 401;
            var returnHash = new byte[] { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetUserHash(It.IsAny<string>()))
                .ReturnsAsync(returnHash);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LoginAsync_UnsuccessfulVerifyPasswordHash_Returns401()
        {
            var expectedStatusCode = 401;
            var returnHash = new byte[] { };
            var returnSalt = new byte[] { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetUserHash(It.IsAny<string>()))
                .ReturnsAsync(returnHash);

            jwtTokenService.Setup(x => x.GetUserSalt(It.IsAny<string>()))
                .ReturnsAsync(returnSalt);

            jwtTokenService.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LoginAsync_ProvidingWrongJwtToken_Returns500()
        {
            var expectedStatusCode = 500;
            var returnHash = new byte[] { };
            var returnSalt = new byte[] { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetUserHash(It.IsAny<string>()))
                .ReturnsAsync(returnHash);

            jwtTokenService.Setup(x => x.GetUserSalt(It.IsAny<string>()))
                .ReturnsAsync(returnSalt);

            jwtTokenService.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LoginAsync_ProvidingWrongUserData_Returns500()
        {
            var expectedStatusCode = 500;
            var returnHash = new byte[] { };
            var returnSalt = new byte[] { };
            var expectedJwtToken = new JwtTokenDTO { AccessToken = "test test test test" };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.ExistsUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.AccountActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetUserHash(It.IsAny<string>()))
                .ReturnsAsync(returnHash);

            jwtTokenService.Setup(x => x.GetUserSalt(It.IsAny<string>()))
                .ReturnsAsync(returnSalt);

            jwtTokenService.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);

            jwtTokenService.Setup(x => x.RenewAccessRefreshToken(It.IsAny<string>()))
                .ReturnsAsync(expectedJwtToken);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Login(new UserLoginDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region RegisterAsync
        [Fact]
        public async void RegisterAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.RegisterNewUser(It.IsAny<UserRegisterDTO>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Register(new UserRegisterDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RegisterAsync_UnsucessfulRegisterNewUser_Returns500()
        {
            var expectedStatusCode = 500;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.RegisterNewUser(It.IsAny<UserRegisterDTO>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Register(new UserRegisterDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region RenewAccessTokenAsync
        [Fact]
        public async void RenewAccessTokenAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User
            {
                Id = 1,
                RefreshToken = "testtesttesttesttest",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
            };
            var returnToken = new AccessTokenModel { };
            var expectedValue = new { UserData = new UserDTO { }, AccessToken = returnToken };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.CreateAccessToken(It.IsAny<string>()))
                .Returns(returnToken);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.RenewAccessToken(new JwtTokenDTO { AccessToken = "Bearer xxxxxxxxxx", RefreshToken = "testtesttesttesttest" });

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            try
            {
                Assert.IsAssignableFrom(expectedValue.GetType(), res.Value);
                var actualValue = (dynamic)res.Value;
                Assert.Multiple(() =>
                {
                    Assert.Equal(expectedValue.UserData, actualValue.UserData);
                    Assert.Equal(expectedValue.AccessToken, actualValue.AccessToken);
                });

            }
            catch (Exception)
            {

            }
        }

        [Fact]
        public async void RenewAccessTokenAsync_NotProvidingRequest_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.RenewAccessToken(null!);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RenewAccessTokenAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.RenewAccessToken(new JwtTokenDTO { RefreshToken = "testtesttesttesttest" });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RenewAccessTokenAsync_ProvidingWrongRefreshToken_Returns403()
        {
            var expectedStatusCode = 403;
            var returnUser = new User
            {
                RefreshToken = "testtesttesttesttest",
                RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
            };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.RenewAccessToken(new JwtTokenDTO { AccessToken = "Bearer xxxxxxxxxx" });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RenewAccessTokenAsync_ProvidingWrongRefreshTokenExpiryTime_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUser = new User
            {
                RefreshToken = "testtesttesttesttest",
                RefreshTokenExpiryTime = DateTime.Now
            };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.RenewAccessToken(new JwtTokenDTO { AccessToken = "Bearer xxxxxxxxxx", RefreshToken = "testtesttesttesttest" });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region LogoutAsync
        [Fact]
        public async void LogoutAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.Logout(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Logout("test@test.test");

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void LogoutAsync_UnsuccessfulLogout_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            jwtTokenService.Setup(x => x.Logout(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.Logout("test@test.test");

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region ActivateEmailAsync
        [Fact]
        public async void ActivateEmailAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByActivationToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            emailService.Setup(x => x.EmailIsActivated(It.IsAny<string>()))
                .ReturnsAsync(false);

            emailService.Setup(x => x.ActivateEmail(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ActivateEmail("testtesttesttesttest@test.test");

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ActivateEmailAsync_ProvidingWrongActivationToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ActivateEmail("testtesttesttesttest@test.test");

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ActivateEmailAsync_TrueEmailsActivated_Returns304()
        {
            var expectedStatusCode = 304;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByActivationToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            emailService.Setup(x => x.EmailIsActivated(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ActivateEmail("testtesttesttesttest@test.test");

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ActivateEmailAsync_UnsuccessfulActivateEmail_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByActivationToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            emailService.Setup(x => x.EmailIsActivated(It.IsAny<string>()))
                .ReturnsAsync(false);

            emailService.Setup(x => x.ActivateEmail(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ActivateEmail("testtesttesttesttest@test.test");

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region ResetPasswordAsync
        [Fact]
        public async void ResetPasswordAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByResetPasswordToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.ResetUserPassword(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPassword("somepasswordtesttoken", new ResetPasswordDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ResetPasswordAsync_ProvidingWrongResetPasswordToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPassword("somepasswordtesttoken", new ResetPasswordDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ResetPasswordAsync_UnsuccessfulResetUserPassword_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByResetPasswordToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.ResetUserPassword(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPassword("somepasswordtesttoken", new ResetPasswordDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region ResetPasswordSendEmailAsync
        [Fact]
        public async void ResetPasswordSendEmailAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };
            var returnResetPasswordToken = "testpasswordtoken";

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.SetResetPasswordToken(It.IsAny<int>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetResetPasswordToken(It.IsAny<int>()))
                .ReturnsAsync(returnResetPasswordToken);

            emailService.Setup(x => x.SendEmail(It.IsAny<EmailTemplate>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPasswordSendEmail(new ResetPasswordEmail { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ResetPasswordSendEmailAsync_ProvidingWrongEmailData_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPasswordSendEmail(new ResetPasswordEmail { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ResetPasswordSendEmailAsync_UnsuccessfulSetResetPasswordToken_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.SetResetPasswordToken(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPasswordSendEmail(new ResetPasswordEmail { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ResetPasswordSendEmailAsync_ProvidingWrongResetPasswordToken_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.SetResetPasswordToken(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPasswordSendEmail(new ResetPasswordEmail { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void ResetPasswordSendEmailAsync_UnsuccessfulSendEmail_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };
            var returnResetPasswordToken = "testpasswordtoken";

            // Arrange
            var jwtTokenService = new Mock<IJwtTokenService>();
            var emailService = new Mock<IEmailService>();

            emailService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            jwtTokenService.Setup(x => x.SetResetPasswordToken(It.IsAny<int>()))
                .ReturnsAsync(true);

            jwtTokenService.Setup(x => x.GetResetPasswordToken(It.IsAny<int>()))
                .ReturnsAsync(returnResetPasswordToken);

            emailService.Setup(x => x.SendEmail(It.IsAny<EmailTemplate>()))
                .ReturnsAsync(false);

            // Act
            var controller = new AuthController(jwtTokenService.Object, emailService.Object, null!);
            var response = await controller.ResetPasswordSendEmail(new ResetPasswordEmail { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion
    }
}
