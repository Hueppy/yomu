using GenFu;
using Microsoft.EntityFrameworkCore;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;
using Yomu.Shared.Services;

var pwHash = new PasswordHasher().HashPassword(null, "example");

var opt = new DbContextOptionsBuilder<YomuContext>();
opt.EnableSensitiveDataLogging();

using var context = new YomuContext(opt.Options);

void Generate<T>(DbSet<T> set, int count = 25)
	where T : class, new()
{
    var l = A.ListOf<T>(count);
	set.AddRange(l);
}

void GenerateDistinct<T, TKey>(DbSet<T> set, Func<T, TKey> keySelector, int limit = 25)
	where T : class, new()
{
	set.AddRange(A.ListOf<T>(limit).DistinctBy(keySelector));	
}

HashSet<T> SelectRandom<T>(IEnumerable<T> collection, int limit = 10)
{
	var collectionCount = collection.Count();
	var random = GenFu.GenFu.Random;
	var count = Math.Min(random.Next(limit), collectionCount);
	var list = new HashSet<T>();
	for (int i = 0; i < count; i++) {
		list.Add(collection.ElementAt(random.Next(collectionCount)));
	}
	return list;
}

GenFu.GenFu.Configure<Login>()
    .Fill(x => x.PasswordHash, pwHash);
GenFu.GenFu.Configure<Community>()
    .Fill(x => x.Description).AsLoremIpsumSentences();

Generate(context.Logins);
GenerateDistinct(context.Communities, x => x.Id, 15);

await context.SaveChangesAsync();

var emails = context.Logins.Select(x => x.Email).AsEnumerable();
var communityIds = context.Communities.Select(x => x.Id).AsEnumerable();

GenFu.GenFu.Configure<User>()
    .Fill(x => x.Id).AsFirstName()
    .Fill(x => x.Email).WithRandom(emails);

GenerateDistinct(context.Users, x => x.Id, 25);

await context.SaveChangesAsync();

var userIds = context.Users.Select(x => x.Id).AsEnumerable();

GenFu.GenFu.Configure<UserCommunity>()
    .Fill(x => x.CommunityId).WithRandom(communityIds)
    .Fill(x => x.UserId).WithRandom(userIds);
GenFu.GenFu.Configure<Message>()
    .Fill(x => x.Id, 0)
    .Fill(x => x.SenderId).WithRandom(userIds)
    .Fill(x => x.ReceiverId).WithRandom(userIds)
    .Fill(x => x.Text).AsLoremIpsumSentences();
GenFu.GenFu.Configure<Post>()
    .Fill(x => x.Id, 0)
    .Fill(x => x.CommunityId).WithRandom(communityIds)
    .Fill(x => x.UserId).WithRandom(userIds)
    .Fill(x => x.Text).AsLoremIpsumSentences();

GenerateDistinct(context.UserCommunities, x => $"{x.CommunityId}{x.UserId}", 200);
Generate(context.Messages, 200);
Generate(context.Posts, 200);

await context.SaveChangesAsync();

var postIds = context.Posts.Select(x => x.Id).AsEnumerable();

GenFu.GenFu.Configure<PostRating>()
    .Fill(x => x.PostId).WithRandom(postIds)
    .Fill(x => x.UserId).WithRandom(userIds);
GenFu.GenFu.Configure<Report>()
    .Fill(x => x.Id, 0)
    .Fill(x => x.SenderId).WithRandom(userIds)
    .Fill(x => x.Comment).AsLoremIpsumSentences()
/*    .Fill(x => x.Comments, (x) => SelectRandom(context.Comments))
    .Fill(x => x.Posts, (x) => SelectRandom(context.Posts))
    .Fill(x => x.Users, (x) => SelectRandom(context.Users))*/;
GenFu.GenFu.Configure<Comment>()
    .Fill(x => x.Id, 0)
    .Fill(x => x.PostId).WithRandom(postIds)
    .Fill(x => x.UserId).WithRandom(userIds)
    .Fill(x => x.ParentId, () => null)
    .Fill(x => x.Message).AsLoremIpsumSentences();

GenerateDistinct(context.PostRatings, x => $"{x.PostId}{x.UserId}", 1000);
Generate(context.Reports);
Generate(context.Comments, 500);

await context.SaveChangesAsync();

for (int i = 0; i < 5; i++)
{
    var commentIds = context.Comments.Select(x => (int?)x.Id).AsEnumerable();
    GenFu.GenFu.Configure<Comment>()
        .Fill(x => x.ParentId).WithRandom(commentIds);

    Generate(context.Comments, 100);
    await context.SaveChangesAsync();        
}
