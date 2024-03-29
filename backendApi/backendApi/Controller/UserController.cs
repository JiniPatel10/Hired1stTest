﻿#region Usings
using Amazon.Runtime;
using backendApi.Infrastructure;
using backendApi.Middleware;
using core.Domain.ClassTypes;
using core.Domain.Models;
using core.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace backendApi.Controller
{
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        #region Private variables
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        #endregion

        #region Constructor
        public UserController(IMailService mailService,
                   IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mailService = mailService;

        }
        #endregion
        #region Public methods

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                User existingUser = await _userRepository.GetUserByEmail(user.Email);
                if (existingUser == null)
                {
                    user.Password = setPasswordToBase64(user.Password);
                    user.Created = DateTime.Now;
                    user = await _userRepository.Save(user);
                    user.Password = getPasswordFromBase64(user.Password);
                }
                else
                {
                    return UnprocessableEntity("User with this email already exists.");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = "Error occur while creating user in database" + ex.Message;
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.CreatingUserError, errorMessage));
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            try
            {
                model.Password = setPasswordToBase64(model.Password);
                var user = await _userRepository.CheckUser(model.Email, model.Password);

                if (user == null)
                    return UnprocessableEntity("Your registered account information was not recognized. Please enter your email and password again");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING");
                var expires = DateTime.Now.AddHours(1);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.FirstName)
                    }),
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string securityToken = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    token = securityToken,
                    expiresIn = expires,
                    id = user.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.ValidatingUserError, ex.Message));

            }

        }
        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var result = await _userRepository.GetById(id);

                if (result == null)
                    return NotFound("User not found");
                else
                    result.Password = getPasswordFromBase64(result.Password);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                string ErrorMsg = "Error getting user by id" + ex.Message;
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.GettingUserByIdError, ErrorMsg));
            }
        }
        [HttpPost("updateUserEmail")]
        public async Task<IActionResult> UpdateUserEmail([FromBody] User model)
        {
            try
            {
                User user = await _userRepository.GetUserByEmail(model.Email);
                if (user == null)
                {
                    await _userRepository.ChangeEmail(model.Id, model.Email);
                    return new JsonResult(model);
                }
                else
                {
                    return UnprocessableEntity("Email already exists. New email should be different than the existing email. Please check your entry and try again.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.UpdatingUserError, ex.Message));
            }

        }
        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User model)
        {
            try
            {
                User user = await _userRepository.GetById(model.Id);
                if (model.Email.ToLower().Trim() != user.Email.ToLower().Trim())
                {
                     user = await _userRepository.GetUserByEmail(model.Email);
                    if(user == null)
                    {
                        await _userRepository.UpdateUser(model);
                        return new JsonResult(model);
                    }
                    else
                    {
                        return UnprocessableEntity("Email already exists. Please check your entry and try again.");

                    }
                }
                else
                {
                    await _userRepository.UpdateUser(model);
                    return new JsonResult(model);
                }
         

            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.UpdatingUserError, ex.Message));
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _userRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string ErrorMsg = "Error deleting user by id" + ex.Message;
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.DeletingUserError, ErrorMsg));
            }
        }
        [HttpPost("getUsersList")]
        public async Task<IActionResult> GetUsersList([FromBody] PageInput pageInput)
        {
            try
            {
                PageResult<User> ContactList = await _userRepository.GetUserList(pageInput);
                return Ok(ContactList);
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occur while getting the user list  data by id" + ex.Message;
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.GettingUserByIdError, errorMessage));
            }
        }
        [HttpPost("addNewUser")]
        public async Task<IActionResult> AddNewUser([FromBody] User user)
        {
            try
            {
                User existingUser = await _userRepository.GetUserByEmail(user.Email);
                if (existingUser == null)
                {
                    string password = GenerateRandomPassword(6);
                    user.Password = setPasswordToBase64(password);
                    user.Created = DateTime.Now;
                    User newUser = await _userRepository.Save(user);
                    return Ok(newUser);

                }
                else
                {
                    return UnprocessableEntity("User with this email already exists.");
                }

            }
            catch (Exception ex)
            {
                string errorMessage = "Error occur while creating user in database" + ex.Message;
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.CreatingUserError, errorMessage));
            }
        }

        [HttpPost("changePassword/{userIdentifier}")]
        public async Task<IActionResult> ChangePassword([FromBody] User model, string userIdentifier)
        {
            try
            {
                model.Password = setPasswordToBase64(model.Password);
                await _userRepository.ChangePassword(userIdentifier, model.Password);
                return new JsonResult(true);


            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.UpdatingUserError, ex.Message));
            }

        }

        [HttpGet("sendForgotPasswordMail/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                User user = await _userRepository.GetUserByEmail(email);
                if (user == null)
                {
                    return UnprocessableEntity("User with this email doesnot exists.");
                }
                else
                {
                    string link = "http://localhost:5000/change-password/" + user.Id;
                    string htmlContent = @"
							<!DOCTYPE html>
<html>
<head>
    <title>Reset Your Password</title>
</head>
<body>
    <p>Hello, " + " " + @"</p>
    <p>You recently requested to reset your password for Hired1st. Please click the button below to reset your password.</p>
    <a href= " + link + @"  style=""display: inline-block; background-color: #007BFF; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px;"">Click Me</a>
<br/>
<hr>
<p>If you did not request your password to be reset, please disregard this email.</p>
<br/>
<p>Thank you,</p>
<p>Hired 1st</p>
</body>
</html>";
                    bool isSent = await _mailService.SendMailForForgotPassword(email, htmlContent);
                    return new JsonResult(isSent);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralErrorResultModel(ErrorCodes.SendMailError, ex.Message));
            }

        }
        #endregion

        #region Private Methods
        private string getPasswordFromBase64(string password)
        {
            var base64Bytes = Convert.FromBase64String(password);
            // decrypted password
            return Encoding.UTF8.GetString(base64Bytes);
        }
        private string setPasswordToBase64(string password)
        {
            // Convert the password to a byte array
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(password);
            // encryoted password
            return Convert.ToBase64String(bytesToEncode);
        }
        public static string GenerateRandomPassword(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=";

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                char[] chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = allowedChars[randomBytes[i] % allowedChars.Length];
                }

                return new string(chars);
            }
        }
        #endregion
    }
}
