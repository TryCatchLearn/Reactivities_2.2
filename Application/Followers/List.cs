using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Profiles;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class List
    {
        public class Query : IRequest<List<Profile>> 
        { 
            public string Username { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Profile>>
        {
            private readonly DataContext _context;
            private readonly IProfileReader _profileReader;
            public Handler(DataContext context, IProfileReader profileReader)
            {
                _profileReader = profileReader;
                _context = context;
            }

            public async Task<List<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var queryable = _context.Followings.AsQueryable();

                var userFollowings = new List<UserFollowing>();
                var profiles = new List<Profile>();

                switch(request.Predicate)
                {
                    case "followers":
                    {
                        userFollowings = await queryable.Where(x => 
                            x.Target.UserName == request.Username).ToListAsync();

                        foreach(var follower in userFollowings)
                        {
                            profiles.Add(await _profileReader.ReadProfile(follower.Observer.UserName));
                        }
                        break;
                    }
                    case "following":
                    {
                        userFollowings = await queryable.Where(x => 
                            x.Observer.UserName == request.Username).ToListAsync();

                        foreach(var follower in userFollowings)
                        {
                            profiles.Add(await _profileReader.ReadProfile(follower.Target.UserName));
                        }
                        break;
                    }
                }

                return profiles;
            }
        }
    }
}