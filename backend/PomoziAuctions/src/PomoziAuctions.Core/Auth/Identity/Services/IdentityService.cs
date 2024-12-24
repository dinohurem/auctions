using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PomoziAuctions.Core.Abstractions;
using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.Core.Auth.Security.Interfaces;
using PomoziAuctions.Core.Auth.Security.Models;
using PomoziAuctions.SharedKernel.DataFilters;
using System.Security.Cryptography;
using IdentityUser = PomoziAuctions.Core.Auth.Identity.Models.IdentityUser;
using PomoziAuctions.Core.Auth.Identity.Interfaces;

namespace PomoziAuctions.Core.Auth.Identity.Services;

public class IdentityService : IIdentityService
{
  private const string UserInvitationTokenPurpose = "UserInvitation";
  private const string ResetPasswordTokenPurpose = "ResetPassword";

  private readonly UserManager<IdentityUser> _userManager;
  private readonly RoleManager<Models.IdentityRole> _roleManager;
  private readonly IEmailSender _emailSender;
  private readonly ITokenService _tokenService;
  private readonly IDataFilter _dataFilter;
  private readonly IStringEncryptor _stringEncryptor;
  private readonly IOptions<IdentityEmailOptions> _identityEmailOptions;
  private readonly ICurrentUser _currentUser;

  public IdentityService(UserManager<Models.IdentityUser> userManager,
    RoleManager<Models.IdentityRole> roleManager,
    IEmailSender emailSender,
    ITokenService tokenService,
    IDataFilter dataFilter,
    IStringEncryptor stringEncryptor,
    IOptions<IdentityEmailOptions> identityEmailOptions,
    ICurrentUser currentUser)
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _emailSender = emailSender;
    _tokenService = tokenService;
    _dataFilter = dataFilter;
    _stringEncryptor = stringEncryptor;
    _identityEmailOptions = identityEmailOptions;
    _currentUser = currentUser;
  }

  public async Task<Result<List<Models.IdentityUser>>> GetUsers() => new Result<List<Models.IdentityUser>>(_userManager.Users.ToList());

  public async Task<Result<bool>> UpdateUserEmail(string oldEmail, string newEmail)
  {
    var user = await _userManager.FindByEmailAsync(oldEmail);
    if (user == null)
    {
      return new Result<bool>(false);
    }

    user.Email = newEmail;
    user.UserName = newEmail;
    user.NormalizedEmail = _userManager.NormalizeEmail(newEmail);
    user.NormalizedUserName = _userManager.NormalizeName(newEmail);

    var result = await _userManager.UpdateAsync(user);
    if (!result.Succeeded)
    {
      return new Result<bool>(false);
    }

    return new Result<bool>(true);
  }

  // Change roles of a user.
  public async Task<Result<bool>> UpdateUserRoles(string email, ICollection<string> roles)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
      return new Result<bool>(false);
    }

    var userRoles = await _userManager.GetRolesAsync(user);
    var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
    if (!result.Succeeded)
    {
      return new Result<bool>(false);
    }

    result = await _userManager.AddToRolesAsync(user, roles);
    if (!result.Succeeded)
    {
      return new Result<bool>(false);
    }

    return new Result<bool>(true);
  }

  public async Task<Result<AcceptInvitationResponseDto>> AcceptInvitation(AcceptInvitationDto acceptInvitationDto)
  {
    var user = await _userManager.FindByEmailAsync(acceptInvitationDto.Email);
    var code = _stringEncryptor.Decrpyt(acceptInvitationDto.Code, UserInvitationTokenPurpose);

    var result = await _userManager.ResetPasswordAsync(user, code, acceptInvitationDto.Password);
    if (!result.Succeeded)
    {
      return new Result<AcceptInvitationResponseDto>(null);
    }

    var userRoles = await _userManager.GetRolesAsync(user);
    var userPermissions = await _roleManager.GetRolesPermissions(userRoles.ToArray());

    return new AcceptInvitationResponseDto
    {
      CanUseWebApp = userPermissions.Any(p => p == Permissions.CanUseWebApp),
    };
  }

  // TODO: Change response type.
  public async Task<Result<bool>> SendResetPasswordEmail(string email, string language)
  {
    var user = await _userManager.FindByEmailAsync(email);

    if (user == null)
    {
      // TODO: Log error but do not leak whether user with email exists or not in the response.
      return new Result<bool>(true);
    }

    // Generate token
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    var encryptedCode = _stringEncryptor.Encrpyt(token, ResetPasswordTokenPurpose);

    // Construct the reset password link
    var resetPasswordLink = $"{_identityEmailOptions.Value.ResetPasswordUrl}?token={System.Net.WebUtility.UrlEncode(encryptedCode)}&email={email}";

    var templateId = _identityEmailOptions.Value.PasswordRecoveryTemplate;

    await _emailSender.SendAsync(_identityEmailOptions.Value.FromEmail, email, templateId, new
    {
      email,
      resetPasswordLink
    });

    return new Result<bool>(true);
  }

  // TODO: Change response type.
  public async Task<Result<bool>> ResetPassword(ResetPasswordDto resetPasswordDto)
  {
    var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

    if (user == null)
    {
      // TODO: Add error message.
      return new Result<bool>(false);
    }

    var token = _stringEncryptor.Decrpyt(resetPasswordDto.Token, resetPasswordDto.IsNewUser ? UserInvitationTokenPurpose : ResetPasswordTokenPurpose);

    var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);
    if (!result.Succeeded)
    {
      return new Result<bool>(false);
    }

    return new Result<bool>(true);
  }

  public async Task<Result<bool>> ChangePassword(ChangePasswordDto changePasswordDto)
  {
    if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
    {
      return Result<bool>.Error("Trenutni password nije tacan.");
    }

    var user = await _userManager.FindByIdAsync(_currentUser.Id?.ToString());

    if (user == null)
    {
      return Result<bool>.Error();
    }

    var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
    if (!result.Succeeded)
    {
      return Result<bool>.Error();
    }

    return new Result<bool>(true);
  }

  private static string GeneratePassword(int length) => Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
}
