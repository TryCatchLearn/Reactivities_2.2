using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class ExternalLogin
    {
        public class Query : IRequest<User>
        {
            public string AccessToken { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IFacebookAccessor _facebookAccessor;
            private readonly UserManager<AppUser> _userManager;
            public Handler(IJwtGenerator jwtGenerator, IFacebookAccessor facebookAccessor, UserManager<AppUser> userManager)
            {
                _userManager = userManager;
                _facebookAccessor = facebookAccessor;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                // get user info and verify token is valid
                var userInfo = await _facebookAccessor.FacebookLogin(request.AccessToken);

                // if user info is null then token could not be validated
                if (userInfo == null)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "Problem validating token" });
                }

                var user = await _userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    user = new AppUser
                    {
                        DisplayName = userInfo.FirstName,
                        Id = userInfo.Id,
                        Email = userInfo.Email,
                        UserName = "fb_" + userInfo.Id
                    };

                    var photo = new Photo
                    {
                        Url = userInfo.Picture.Data.Url,
                        IsMain = true
                    };

                    user.Photos.Add(photo);

                    var result = await _userManager.CreateAsync(user);

                    if (!result.Succeeded) throw new RestException(HttpStatusCode.BadRequest, new { User = "Problem adding user" });
                }

                return new User
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    Username = user.UserName,
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
            }
        }
    }
}